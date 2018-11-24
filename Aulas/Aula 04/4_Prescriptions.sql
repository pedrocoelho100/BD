CREATE SCHEMA Prescriptions;
GO
CREATE TABLE Prescriptions.Paciente(
	Num_Utente INT NOT NULL,
	Nome VARCHAR(50) NOT NULL,
	Data_Nasc DATE NOT NULL,
	Endereço VARCHAR(80),
	PRIMARY KEY (Num_Utente)
);
CREATE TABLE Prescriptions.Medico(
	Num_Identificacao INT NOT NULL,
	Nome VARCHAR(50) NOT NULL,
	Especialidade VARCHAR(20),
	PRIMARY KEY (Num_Identificacao)
);
CREATE TABLE Prescriptions.Farmacia(
	Nome VARCHAR(50) NOT NULL,
	Telefone INT NOT NULL,
	Endereço VARCHAR(80) NOT NULL,
	PRIMARY KEY (Nome)
);
CREATE TABLE Prescriptions.Prescricao(
	Num_Unico INT NOT NULL,
	Medico INT NOT NULL REFERENCES Prescriptions.Medico(Num_Identificacao),
	Paciente INT REFERENCES Prescriptions.Paciente(Num_Utente) NOT NULL,
	Data_Process DATE NOT NULL,
	Local_Process VARCHAR(50) REFERENCES Prescriptions.Farmacia(Nome),
	PRIMARY KEY (Num_Unico),
);
CREATE TABLE Prescriptions.Farmaceutica(
	Num_Registo_Nacional INT NOT NULL,
	Nome VARCHAR(50) NOT NULL,
	Endereco VARCHAR(80),
	PRIMARY KEY (Num_Registo_Nacional)
);
CREATE TABLE Prescriptions.Farmaco(
	Nome VARCHAR(50) NOT NULL,
	Farmaceutica INT REFERENCES Prescriptions.Farmaceutica(Num_Registo_Nacional) NOT NULL,
	Formula VARCHAR(100) NOT NULL,
	PRIMARY KEY (Nome, Farmaceutica)
);
CREATE TABLE Prescriptions.Farmacos_da_Prescriçao(
	Prescriçao INT REFERENCES Prescriptions.Prescriçao(Num_Unico) NOT NULL,
	Nome_Farmaco VARCHAR(50) NOT NULL,
	Farmaceutica_Farmaco INT NOT NULL,
	FOREIGN KEY (Nome_Farmaco, Farmaceutica_Farmaco) REFERENCES Prescriptions.Farmaco(Nome, Farmaceutica),
	PRIMARY KEY (Prescricao, Nome, Farmaceutica_Farmaco)
);
