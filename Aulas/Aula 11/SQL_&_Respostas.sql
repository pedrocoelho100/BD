-- a)
/*
CREATE PROC Remove_Employee @Ssn INT
AS
	BEGIN TRANSACTION;

	BEGIN TRY
		UPDATE department SET mgr_ssn = NULL WHERE mgr_ssn = @Ssn;
		UPDATE employee SET super_ssn = NULL WHERE super_ssn = @Ssn;
		DELETE FROM dependent WHERE essn = @Ssn;
		DELETE FROM works_on WHERE essn = @Ssn;
		DELETE FROM employee WHERE ssn = @Ssn;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		DECLARE @error_msg INT
		SELECT @error_msg = 'An error ocurred when trying to remove employee' + @Ssn;
		RAISERROR (@error_msg, 14, 1);
		ROLLBACK TRANSACTION;
	END CATCH;

EXEC Remove_Employee 21312332;
*/

-- b)
/*
CREATE PROC Mgr_Employee (@Ssn_oldest INT OUTPUT, @years_oldest INT OUTPUT)
AS
	DECLARE @temp TABLE(Fname VARCHAR(15), Minit char(1), Lname VARCHAR(15), Ssn INT, Bdate DATE, Address VARCHAR(30), Sex CHAR(1), Salary DECIMAL(10,2), Mgr_start_date DATE)
	INSERT INTO @temp
		SELECT Fname, Minit, Lname, Ssn, Bdate, Address, Sex, Salary, Mgr_start_date FROM (employee JOIN department ON ssn = Mgr_ssn) ORDER BY Mgr_start_date
	SELECT @Ssn_oldest = Ssn, @years_oldest = DATEDIFF(yy, Mgr_start_date, GETDATE()) FROM @temp  ORDER BY Mgr_start_date DESC

	SELECT Fname, Minit, Lname, Ssn, Bdate, Address, Sex, Salary FROM @temp;
GO

DECLARE @Ssn_oldest INT;
DECLARE @years_oldest INT;

EXEC Mgr_Employee @Ssn_oldest OUTPUT, @years_oldest OUTPUT;
PRINT @Ssn_oldest;
PRINT @years_oldest;
*/

--c
/*
CREATE TRIGGER Mgr_Single_Department ON department AFTER INSERT, UPDATE
AS
	DECLARE @max_occur INT;
	SELECT @max_occur = MAX(ssn_count) FROM (SELECT Count(Mgr_ssn) as ssn_count FROM department GROUP BY Mgr_ssn) AS SsnCount;
	IF @max_occur > 1
		BEGIN
			RAISERROR('Funcionario nao pode ser gestor de mais do que um departamento', 16, 1);
			ROLLBACK TRAN;
		END
GO

UPDATE department SET Mgr_ssn = 321233765 WHERE Dnumber = 1;
*/

--d
/*
CREATE TRIGGER Mgr_Employee_Salary ON employee AFTER INSERT, UPDATE
AS

	DECLARE @salary MONEY;

	SELECT @salary = employee.Salary FROM employee JOIN (department JOIN inserted ON department.Dnumber = inserted.Dno) ON employee.Ssn = department.Mgr_ssn;

	DECLARE @newSalary MONEY;
	DECLARE @ssn INT;

	SELECT @ssn = inserted.Ssn, @newSalary = inserted.Salary FROM inserted;

	IF @newSalary >= @salary
	BEGIN
		 UPDATE employee SET Salary = @salary - 1 WHERE employee.Ssn = @ssn;
	END;

GO
UPDATE employee SET Salary = 1300 WHERE Ssn = 321233765;
*/

-- e)
/*
CREATE FUNCTION EmployeeProjects(@ssn INT)
RETURNS TABLE
WITH SCHEMABINDING, ENCRYPTION
AS
	RETURN (SELECT Pname, Plocation FROM (dbo.works_on JOIN dbo.project ON works_on.Pno = project.Pnumber) WHERE works_on.Essn = @ssn);

GO
SELECT * FROM EmployeeProjects(183623612);
*/

-- f)
/*
CREATE FUNCTION employee_salary(@dnumber INT)
RETURNS @table TABLE ("fname" VARCHAR(50), "minit" VARCHAR(50), "lname" VARCHAR(50), "ssn" INT, "salary" MONEY)
WITH SCHEMABINDING, ENCRYPTION
AS
	BEGIN
		INSERT @table SELECT Fname, Minit, Lname, Ssn, Salary FROM dbo.employee where Dno = @dnumber AND Salary > (SELECT AVG(salary) FROM dbo.employee WHERE Dno = @dnumber);
		RETURN;
	END;

GO
SELECT * FROM employee_salary(2);
*/

-- g)
/*
CREATE FUNCTION employeeDeptHighAverage(@Dno INT)
RETURNS @table TABLE ("pname" VARCHAR(100), "pnumber" INT, "plocation" VARCHAR(100), "dnum" INT, "budget" MONEY, "totalbudget" MONEY)
WITH SCHEMABINDING, ENCRYPTION
AS
BEGIN
	DECLARE @pname VARCHAR(100), @pnumber INT, @plocation VARCHAR(100), @dnum INT, @budget MONEY, @totalbudget MONEY = 0;

	DECLARE C CURSOR FAST_FORWARD
	FOR SELECT Pname AS 'pname', Pnumber AS 'pnumber', Plocation AS 'plocation',
			Dnum AS 'dnum', SUM(works_on.Hours*employee.Salary/40) AS 'budget'
			FROM dbo.project JOIN (dbo.works_on JOIN
					dbo.employee ON works_on.Essn = employee.Ssn)
					ON project.Pnumber = works_on.Pno
			WHERE project.Dnum = @Dno
			GROUP BY project.Pnumber, project.Pname, project.Plocation, project.Dnum;

	OPEN C;

	FETCH C INTO @pname, @pnumber, @plocation, @dnum, @budget;

	WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @totalbudget +=  @budget;
			INSERT INTO @table(pname, pnumber, plocation, dnum, budget, totalbudget)
				   VALUES (@pname, @pnumber, @plocation, @dnum, @budget, @totalbudget);
			FETCH C INTO @pname, @pnumber, @plocation, @dnum, @budget;
		END;

	CLOSE C;
	RETURN
END;

GO
SELECT * FROM employeeDeptHighAverage(3);
*/

-- h)
/*
-- DROP TRIGGER trigger_delete_department
CREATE TRIGGER trigger_delete_department ON dbo.department
INSTEAD OF DELETE
AS
	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
				WHERE TABLE_SCHEMA = 'company' AND TABLE_NAME = 'department_deleted'))
		BEGIN
			CREATE TABLE dbo.department_deleted(
				Dnumber INT PRIMARY KEY,
				Dname VARCHAR(180) NOT NULL,
				Mgr_ssn CHAR(9) REFERENCES employee(Ssn),
				Mgr_start_date DATE);
		END;

		INSERT INTO department_deleted SELECT * FROM deleted;
		SELECT * from deleted;
GO

DELETE FROM department WHERE Dnumber = 3

The advantage is the fact that the trigger is executed before the reference integrity checks.
*/

-- i) 
/*
As operações de um store procedures são compilados
no momento de criação e não têm de ser recompiladas cada vez que o
procedimento é invocado. Têm execução mais rápida pois são guardados em
memória cache na primeira vez que são executados.
Stored procedures são úteis para restringir as acções do utilizador da base de dados criando para isso
store procedures para inserts, updates e deletes, não lhe dando acesso às tabelas que estão por trás, garantindo a integridade dos dados.

As UDFs também são compiladas e optimizadas, sendo boas para ser utilizadas
para incorporar lógica complexa dentro de uma consulta. Uma boa utilização
para as UDFs é serem usadas para o mesmo efeito que uma vista com o
benefício de que usando WITH SCHEMABINDING que previne a alteração
ou eliminação de objetos utilizados pela função.

No entanto:
  - UDF necessita de retornar um valor (escalar or tabela);
  - Stored procedures pode retornar um escalar, um tabela, nada, ou ainda utilizar variáveis de saída;
  - Stored procedures permitem DML, ao contrário das UDF que apenas suportam selects;
  - UDF podem ser chamadas em procedures, o oposto não se verifica;
  - Stored procedures permitem controlar o fluxo da transação SQL;
  - UDF podem ser utilizadas tanto em WHERE/HAVING/SELECT como em JOINs.

  - As UDF's podem ser vistas como views parametrizáveis!
*/