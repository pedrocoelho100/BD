CREATE SCHEMA Company

GO

CREATE TABLE Company.EMPLOYEE(
	fname		VARCHAR(64)	NOT NULL,
	minit		CHAR(2)			NOT NULL,
	lname		VARCHAR(32)		NOT NULL,
	ssn			INT				NOT NULL,
	bdate		DATE,
	address		VARCHAR(128),
	sex			CHAR(1),
	salary		INT,
	super_ssn	INT,
	dno			INT				NOT NULL,
	PRIMARY KEY(ssn)
);

ALTER TABLE Company.EMPLOYEE
	ADD CONSTRAINT SUPERFK FOREIGN KEY(super_ssn) REFERENCES Company.EMPLOYEE(ssn);

CREATE TABLE Company.DEPARTMENT(
	dname		VARCHAR(64)	NOT NULL,
	dnumber		INT				NOT NULL,
	mgr_ssn		INT,
	mgr_start_date	DATE,
	PRIMARY KEY(dnumber),
	FOREIGN KEY(mgr_ssn) REFERENCES Company.EMPLOYEE(ssn)
);

ALTER TABLE Company.EMPLOYEE
	ADD CONSTRAINT DEPFK FOREIGN KEY(dno) REFERENCES Company.DEPARTMENT(dnumber);

CREATE TABLE Company.DEPT_LOCATIONS(
	dnumber		INT				NOT NULL,
	dlocation	VARCHAR(128)	NOT NULL,
	PRIMARY KEY(dnumber, dlocation),
	FOREIGN KEY(dnumber) REFERENCES Company.DEPARTMENT(dnumber)
);

CREATE TABLE Company.DEPENDENTS(
	essn			INT				NOT NULL,
	dependent_name	VARCHAR(128)	NOT NULL,
	sex				CHAR(1),
	bdate			DATE,
	relationship	VARCHAR(128)	NOT NULL,
	PRIMARY KEY(essn, dependent_name),
	FOREIGN KEY(essn) REFERENCES Company.EMPLOYEE(ssn)
);

CREATE TABLE Company.PROJECT(
	pname		VARCHAR(128)	NOT NULL,
	pnumber		INT				NOT NULL,
	plocation	VARCHAR(128)	NOT NULL,
	dnum		INT				NOT NULL,
	PRIMARY KEY(pnumber),
	FOREIGN KEY(dnum) REFERENCES Company.DEPARTMENT(dnumber)
);

CREATE TABLE Company.WORKS_ON(
	essn		INT				NOT NULL,
	pno			INT				NOT NULL,
	hours		DECIMAL				NOT NULL,
	PRIMARY KEY(essn, pno),
	FOREIGN KEY(essn) REFERENCES Company.EMPLOYEE(ssn),
	FOREIGN KEY(pno) REFERENCES Company.PROJECT(pnumber)
);

INSERT INTO Company.DEPARTMENT(dname,dnumber,mgr_ssn,mgr_start_date) VALUES
	('Investigação',1,null,null),
	('Comercial',2,null,null),
	('Logistica',3,null,null),
	('Recursos Humanos',4,null,null),
	('Desporto',5,null,null);

INSERT INTO Company.DEPT_LOCATIONS(dnumber,dlocation) VALUES
	(2,'Aveiro'),
	(3,'Coimbra');

INSERT INTO Company.EMPLOYEE(fname,minit,lname,ssn,bdate,address,sex,salary,super_ssn,dno) VALUES
	('Paula','A','Sousa',183623612,'2001-08-11','Rua da FRENTE','F',1450,null,3),
	('Carlos','D','Gomes',21312332,'2000-01-01','Rua XPTO','M',1200,null,1),
	('Juliana','A','Amaral',321233765,'1980-08-11','Rua BZZZZ','F',1350,null,3),
	('Maria','I','Pereira',342343434,'2001-05-01','Rua JANOTA','F',1250,21312332,2),
	('Joao','G','Costa',41124234,'2001-01-01','Rua YGZ','M',1300,21312332,2),
	('Ana','L','Silva',12652121,'1990-03-03','Rua ZIG ZAG','F',1400,21312332,2);

UPDATE Company.DEPARTMENT SET mgr_ssn = 21312332, mgr_start_date = '2010-08-02' WHERE dnumber = 1;
UPDATE Company.DEPARTMENT SET mgr_ssn = 321233765, mgr_start_date = '2013-05-16' WHERE dnumber = 2;
UPDATE Company.DEPARTMENT SET mgr_ssn = 41124234, mgr_start_date = '2013-05-16' WHERE dnumber = 3;
UPDATE Company.DEPARTMENT SET mgr_ssn = 12652121, mgr_start_date = '2014-04-02' WHERE dnumber = 4;

INSERT INTO Company.DEPENDENTS(essn,dependent_name,sex,bdate,relationship) VALUES
	(21312332,'Joana Costa','F','2008-04-01','Filho'),
	(21312332,'Maria Costa','F','1990-10-05','Neto'),
	(21312332,'Rui Costa','M','	2000-08-04','Neto'),
	(321233765,'Filho Lindo','M','2001-02-22','Filho'),
	(342343434,'Rosa Lima','F','2006-03-11','Filho'),
	(41124234,'Ana Sousa','F','2007-04-13','Neto'),
	(41124234,'Gaspar Pinto','M','2006-02-08','Sobrinho');

INSERT INTO Company.PROJECT(pname,pnumber,plocation,dnum) VALUES
	('Aveiro Digital',1,'Aveiro',3),
	('BD Open Day',2,'Espinho',2),
	('Dicoogle',3,'Aveiro',3),
	('GOPACS',4,'Aveiro',3);

INSERT INTO Company.WORKS_ON(essn,pno,hours) VALUES
	(183623612,1,20.0),
	(183623612,3,10.0),
	(21312332,1,20.0),
	(321233765,1,25.0),
	(342343434,1,20.0),
	(342343434,4,25.0),
	(41124234,2,20.0),
	(41124234,3,30.0);

-- a)
   SELECT fname, minit, lname, ssn, pno FROM Company.EMPLOYEE JOIN Company.WORKS_ON ON ssn=essn;

-- b)
   SELECT fname, minit, lname FROM Company.EMPLOYEE AS E JOIN (SELECT ssn FROM Company.EMPLOYEE AS S WHERE fname='Carlos' AND minit='D' AND lname='Gomes') AS EmployeeCDG ON super_ssn=EmployeeCDG.ssn;

-- c)
   SELECT pname, SUM(hours) AS hours FROM Company.PROJECT JOIN Company.WORKS_ON ON pnumber=pno GROUP BY pname;

-- d)
   SELECT fname, minit, lname, ssn FROM Company.EMPLOYEE JOIN (Company.PROJECT JOIN Company.WORKS_ON ON pnumber=pno) ON ssn=essn WHERE hours >= 20 AND pname='Aveiro Digital' AND dno=3;

-- e)
   SELECT fname, lname FROM Company.EMPLOYEE LEFT OUTER JOIN Company.WORKS_ON ON ssn=essn WHERE pno IS NULL;

-- f)
   SELECT dname, AVG(salary) AS salary_med FROM Company.DEPARTMENT JOIN Company.EMPLOYEE ON dnumber=dno WHERE sex='F' GROUP BY dname;

-- g)
   SELECT fname, lname, ssn FROM Company.EMPLOYEE JOIN Company.DEPENDENTS ON ssn=essn GROUP BY fname, lname, ssn HAVING COUNT(essn) >= 2;

-- h)
   SELECT fname, lname, ssn FROM Company.EMPLOYEE RIGHT OUTER JOIN ((Select * FROM Company.DEPARTMENT WHERE mgr_ssn IS NOT NULL) AS dpts LEFT OUTER JOIN Company.DEPENDENTS ON mgr_ssn=essn) ON ssn=mgr_ssn WHERE essn IS NULL;

-- i)
   SELECT fname, lname, address FROM Company.EMPLOYEE JOIN (Company.WORKS_ON JOIN (Company.DEPT_LOCATIONS JOIN Company.PROJECT ON dnumber=dnum) ON pno=pnumber) ON ssn=essn WHERE plocation = 'Aveiro' AND dlocation != 'Aveiro';
