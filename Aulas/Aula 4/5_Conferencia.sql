CREATE SCHEMA Conferencia;
GO
CREATE TABLE Conferencia.Instituiçao(
	Nome_Instituiçao VARCHAR(50) NOT NULL,
	Endereço VARCHAR(80) NOT NULL,
	PRIMARY KEY (Nome_Instituiçao)
);
CREATE TABLE Conferencia.Pessoa(
	Email VARCHAR(50) NOT NULL,
	Nome VARCHAR(50) NOT NULL,
	Instituiçao VARCHAR(50) REFERENCES Conferencia.Instituiçao(Nome_Instituiçao) NOT NULL,
	PRIMARY KEY (Email)
);
CREATE TABLE Conferencia.Participante(
	Dados_Pessoais VARCHAR(50) REFERENCES Conferencia.Pessoa(Email),
	Endereço VARCHAR(80) UNIQUE NOT NULL,
	PRIMARY KEY (Dados_Pessoais)
);
CREATE TABLE Conferencia.Participante_Estudante(
	Dados_Pessoais VARCHAR(50) REFERENCES Conferencia.Participante(Dados_Pessoais),
	Loc_Comprovativo VARCHAR(100) NOT NULL,
	PRIMARY KEY (Dados_Pessoais)
);
CREATE TABLE Conferencia.Artigo_Cientifico(
	Num_Registo INT,
	Titulo VARCHAR(50) NOT NULL,
	PRIMARY KEY (Num_Registo)
);
CREATE TABLE Conferencia.Escritores(
	Escritor VARCHAR(50) REFERENCES Conferencia.Pessoa(Email),
	Artigo INT REFERENCES Conferencia.Artigo_Cientifico(Num_Registo),
	PRIMARY KEY (Escritor, Artigo)
);
CREATE TABLE Conferencia.Conferencia(
	Nome VARCHAR(30) PRIMARY KEY
);
CREATE TABLE Conferencia.Artigos_Conf(
	Conferencia VARCHAR(30) REFERENCES Conferencia.Conferencia(Nome),
	Artigo INT REFERENCES Conferencia.Artigo_Cientifico(Num_Registo),
	PRIMARY KEY (Conferencia, Artigo)
);
CREATE TABLE Conferencia.Inscritos(
	Conferencia VARCHAR(30) REFERENCES Conferencia.Conferencia(Nome),
	Participante VARCHAR(50) REFERENCES Conferencia.Participante(Dados_Pessoais),
	Data_Inscriçao DATE NOT NULL,
	Referencia INT,
	PRIMARY KEY (Conferencia, Participante)
);
