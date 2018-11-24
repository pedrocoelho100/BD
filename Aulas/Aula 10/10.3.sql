CREATE UNIQUE CLUSTERED INDEX IX_Employee_Ssn ON Employee(Ssn);
CREATE INDEX IX_Employee_Fname_Lname ON Employee(Fname, Lname);
-- Dependendo do nº de departamentos diferentes, poderá não se justificar a criação de um index para estes
CREATE INDEX IX_Employee_Dno ON Employee(Dno);
-- Em semelhança ao anterior, a necessidade de criação de um index para os projetos dependerá do nº de projetos diferentes, entre outros.
CREATE INDEX IX_WorksOn_Pno_Covering_Essn ON Works_On(Pno) INCLUDE (Essn);
CREATE INDEX IX_Dependent_Essn ON Dependent(Essn);
CREATE INDEX IX_Project_Dnum ON Project(Dnum);