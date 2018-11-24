CREATE SCHEMA University;
GO
CREATE TABLE University.Pessoa(
	MEC INT PRIMARY KEY,
	Nome VARCHAR(50) NOT NULL,
	Data_Nascimento DATE NOT NULL,
	Departamento VARCHAR(30) NOT NULL -- REFERENCES University.Departamento(Nome)
);
CREATE TABLE University.Professor(
	Dados_Pessoais INT REFERENCES University.Pessoa(MEC),
	Categoria_Profissional VARCHAR(30) NOT NULL,
	Area_Cientifica VARCHAR(30) NOT NULL,
	Dedicaçao INT NOT NULL,
	PRIMARY KEY (Dados_Pessoais)
);
CREATE TABLE University.Estudante_Graduado(
	Dados_Pessoais INT REFERENCES University.Pessoa(MEC),
	Grau_Formaçao VARCHAR(20) NOT NULL,
	PRIMARY KEY (Dados_Pessoais),
	Advisor INT REFERENCES University.Estudante_Graduado(Dados_Pessoais) -- NOT NULL
	/* Se Advisor for NOT NULL, como seria criado o 1º Advisor? */
);
CREATE TABLE University.Departamento(
	Nome VARCHAR(30) PRIMARY KEY,
	Localizaçao VARCHAR(50) UNIQUE NOT NULL,
	Diretor INT REFERENCES University.Professor (Dados_Pessoais) UNIQUE NOT NULL,
);

ALTER TABLE University.Pessoa ADD FOREIGN KEY (Departamento) REFERENCES University.Departamento(Nome);

CREATE TABLE University.Projeto(
	ID INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(50) NOT NULL,
	Entidade_Financiadora VARCHAR(50),
	Data_Inicio DATE NOT NULL,
	Data_Fim DATE NOT NULL,
	Orçamento SMALLMONEY CHECK (Orçamento >= 0),
	Gestor INT REFERENCES University.Professor(Dados_Pessoais) NOT NULL,
);
CREATE TABLE University.Participaçao_Estudante_Graduado(
	Projeto INT REFERENCES University.Projeto(ID),
	Estudante_Graduado INT REFERENCES University.Estudante_Graduado(Dados_Pessoais),
	Supervisionador INT REFERENCES University.Professor(Dados_Pessoais),
	PRIMARY KEY (Projeto, Estudante_Graduado, Supervisionador)
);
CREATE TABLE University.Participaçao_Professores(
	Projeto INT REFERENCES University.Projeto(ID),
	Professor INT REFERENCES University.Professor(Dados_Pessoais),
	PRIMARY KEY (Projeto, Professor)
);
