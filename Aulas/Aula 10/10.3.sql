CREATE UNIQUE CLUSTERED INDEX IX_Employee_Ssn ON Employee(Ssn);
CREATE INDEX IX_Employee_Fname_Lname ON Employee(Fname, Lname);
-- Dependendo do n� de departamentos diferentes, poder� n�o se justificar a cria��o de um index para estes
CREATE INDEX IX_Employee_Dno ON Employee(Dno);
-- Em semelhan�a ao anterior, a necessidade de cria��o de um index para os projetos depender� do n� de projetos diferentes, entre outros.
CREATE INDEX IX_WorksOn_Pno_Covering_Essn ON Works_On(Pno) INCLUDE (Essn);
CREATE INDEX IX_Dependent_Essn ON Dependent(Essn);
CREATE INDEX IX_Project_Dnum ON Project(Dnum);