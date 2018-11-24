CREATE SCHEMA CinemasZOZ;
GO

CREATE TABLE CinemasZOZ.Distrito(
	ID_Distrito INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(20) UNIQUE NOT NULL
);
CREATE TABLE CinemasZOZ.Cinema(
	ID_Cinema INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(30) UNIQUE NOT NULL CHECK(LEN(nome) >= 3),
	Morada VARCHAR(100) NOT NULL,
	Codigo_Postal CHAR(8) NOT NULL CHECK(cod_postal LIKE '[1-9][0-9][0-9][0-9]-[0-9][0-9][0-9]'),
	ID_Distrito INT NOT NULL REFERENCES CinemasZOZ.Distrito(ID_Distrito),
	Telefone INT NOT NULL CHECK(Telefone LIKE REPLICATE('[0-9]', 9)),
	Gerente INT NOT NULL IDENTITY(1,1) REFERENCES CinemasZOZ.Empregado(ID_Empregado)
);
CREATE TABLE CinemasZOZ.Filme(
	ID_Filme INT PRIMARY KEY IDENTITY(1,1),
	Titulo_Original VARCHAR(70) UNIQUE NOT NULL,
	Titulo_PT VARCHAR(70) UNIQUE NOT NULL,
	Idade_Minima INT NOT NULL CHECK(Idade_Minima >= 6 AND Idade_Minima <= 18),
	Duraçao INT NOT NULL CHECK(Duraçao > 0 AND len(Duraçao) > 0 AND len(Duraçao) <= 3)),
	Data_Estreia DATE NOT NULL,
	Idioma CHAR(2) NOT NULL CHECK(Idioma LIKE '[a-zA-Z][a-zA-Z]'),
	URL_Cartaz VARCHAR(200) CHECK(dbo.IsValidURL(URL_Cartaz) = 1),
	Sinopse VARCHAR(1000) NOT NULL,
	Codigo_IMDB CHAR(9) CHECK(Codigo_IMDB LIKE 'tt[0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	ID_Tecnologia INT NOT NULL IDENTITY(1,1),
	ID_Distribuidora INT NOT NULL IDENTITY(1,1) REFERENCES CinemasZOZ.Distribuidora(ID_Distribuidora)
);
CREATE TABLE CinemasZOZ.Distribuidora(
	ID_Distribuidora INT PRIMARY KEY IDENTITY(1,1),
	Pagamento_Inicial MONEY NOT NULL CHECK(Pagamento_Inicial > 0),
	Comissao_Por_Bilhete SMALLMONEY NOT NULL CHECK(Comissao_Por_Bilhete > 0)
);
CREATE TABLE CinemasZOZ.Tecnologia(
	ID_Tecnologia INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(7) UNIQUE NOT NULL CHECK(len(nome) >= 2)
);
CREATE TABLE CinemasZOZ.Empregado(
	ID_Empregado INT PRIMARY KEY IDENTITY(1,1),
	Salario SMALLMONEY NOT NULL CHECK(Salario > 0),
	Cinema INT NOT NULL IDENTITY(1,1) REFERENCES CinemasZOZ.Cinema(ID_Cinema),
	NIF_Empregado INT UNIQUE NOT NULL CHECK(NIF LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') REFERENCES CinemasZOZ.Pessoa(NIF)
);
CREATE TABLE CinemasZOZ.Sessao(
	ID_Sessao INT IDENTITY(1,1) REFERENCES CinemasZOZ.Filme(ID_Filme),
	ID_Cinema INT IDENTITY(1,1) REFERENCES CinemasZOZ.Cinema(ID_Cinema),
	Dia_Semana WEEKDAY NOT NULL,
	Hora TIME NOT NULL,
	Desconto SMALLMONEY CHECK(Desconto >= 0),
	PRIMARY KEY(ID_Sessao, ID_Cinema, Dia_Semana, Hora)
);
CREATE TABLE CinemasZOZ.Tipo_Bilhete(
	ID_Tipo_Bilhete INT PRIMARY KEY IDENTITY(1,1),
	Descriçao VARCHAR(30) UNIQUE NOT NULL,
	Restriçoes VARCHAR(30) NOT NULL,
	Custo SMALLMONEY NOT NULL CHECK(Custo > 0)
);
CREATE TABLE CinemasZOZ.Sala(
	ID_Sala INT PRIMARY KEY IDENTITY(1,1),
	Capacidade INT NOT NULL CHECK(Capacidade <= 500)
);
CREATE TABLE CinemasZOZ.Tecnologias_Por_Sala(
	ID_Sala INT IDENTITY(1,1) REFERENCES CinemasZOZ.Sala(ID_Sala),
	ID_Tecnologia INT IDENTITY(1,1) REFERENCES CinemasZOZ.Tecnologia(ID_Tecnologia),
	PRIMARY KEY (ID_Sala, ID_Tecnologia)
);
CREATE TABLE CinemasZOZ.Lugar(
	ID_Sala INT IDENTITY(1,1) REFERENCES CinemasZOZ.Sala(ID_Sala),
	Fila VARCHAR(2) CHECK(Fila LIKE '[A-Z]' OR Fila LIKE '[A-Z][A-Z]'),
	Numero INT IDENTITY(1,1),
	Normal_Deficiente BIT NOT NULL,
	PRIMARY KEY(ID_Sala, Fila, Numero)
);
CREATE TABLE CinemasZOZ.Instancia_Sessao(
	ID_Filme INT IDENTITY(1,1) REFERENCES CinemasZOZ.Sessao(ID_Sessao),
	ID_Cinema INT IDENTITY(1,1) REFERENCES CinemasZOZ.Sessao(ID_Cinema),
	Dia_Semana WEEKDAY REFERENCES CinemasZOZ.Sessao(Dia_Semana),
	Hora TIME,
	Dia DATE,
	Sala INT NOT NULL IDENTITY(1,1) REFERENCES CinemasZOZ.Sala(ID_Sala),
	PRIMARY KEY(ID_Filme, ID_Cinema, Dia_Semana, Hora, Dia)
);
CREATE TABLE CinemasZOZ.Bilhete(
	ID_Filme INT IDENTITY(1,1) REFERENCES CinemasZOZ.Instancia_Sessao(ID_Filme),
	ID_Cinema INT IDENTITY(1,1) REFERENCES CinemasZOZ.Instancia_Sessao(ID_Cinema),
	Dia_Semana WEEKDAY REFERENCES CinemasZOZ.Instancia_Sessao(Dia_Semana),
	Hora TIME REFERENCES CinemasZOZ.Instancia_Sessao(Hora),
	Dia DATE REFERENCES CinemasZOZ.Instancia_Sessao(Dia),
	ID_Sala INT IDENTITY(1,1) REFERENCES CinemasZOZ.Lugar(ID_Sala),
	Fila VARCHAR(2) CHECK(Fila LIKE '[A-Z]' OR Fila LIKE '[A-Z][A-Z]') REFERENCES CinemasZOZ.Lugar(Fila),
	Numero INT IDENTITY(1,1) REFERENCES CinemasZOZ.Lugar(Numero),
	ID_Tipo_Bilhete INT IDENTITY(1,1) REFERENCES CinemasZOZ.Tipo_Bilhete(ID_Tipo_Bilhete),
	PRIMARY KEY(ID_Filme, ID_Cinema, Dia_Semana, Hora, Dia, ID_Sala, Fila, Numero)
);
CREATE TABLE CinemasZOZ.Bilhetes_Fatura(
	ID_Fatura INT IDENTITY(1,1),
	ID_Filme INT IDENTITY(1,1) REFERENCES CinemasZOZ.Bilhete(ID_Filme),
	ID_Cinema INT IDENTITY(1,1) REFERENCES CinemasZOZ.Bilhete(ID_Cinema),
	Dia_Semana WEEKDAY REFERENCES CinemasZOZ.Bilhete(Dia_Semana),
	Hora TIME REFERENCES CinemasZOZ.Bilhete(Hora),
	Dia DATE REFERENCES CinemasZOZ.Bilhete(Dia),
	ID_Sala INT IDENTITY(1,1) REFERENCES CinemasZOZ.Bilhete(ID_Sala),
	Fila VARCHAR(2) CHECK(Fila LIKE '[A-Z]' OR Fila LIKE '[A-Z][A-Z]') REFERENCES CinemasZOZ.Bilhete(Fila),
	Numero INT IDENTITY(1,1) REFERENCES CinemasZOZ.Bilhete(Numero),
	PRIMARY KEY(ID_Fatura, ID_Filme, ID_Cinema, Dia_Semana, Hora, Dia, ID_Sala, Fila, Numero)
);
CREATE TABLE CinemasZOZ.Fatura(
	ID_Fatura INT PRIMARY KEY IDENTITY(1,1) REFERENCES Bilhetes_Fatura(ID_Fatura),
	NIF_Cliente INT NOT NULL CHECK(NIF LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') REFERENCES CinemasZOZ.Cliente(NIF_Cliente),
	Data_Compra DATE NOT NULL,
	Hora_Compra TIME NOT NULL,
	Operador INT IDENTITY(1,1) REFERENCES CinemasZOZ.Empregado(ID_Empregado)
);
CREATE TABLE CinemasZOZ.Cliente(
	NIF_Cliente INT PRIMARY KEY CHECK(NIF LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') REFERENCES CinemasZOZ.Pessoa(NIF),
	Morada (Igual à do cinema),
	Codigo Postal (Igual ao do cinema),
	Num_Cartao INT UNIQUE IDENTITY(1,1)
);
CREATE TABLE CinemasZOZ.Pessoa(
	NIF INT PRIMARY KEY CHECK(NIF LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	Nome VARCHAR(50) NOT NULL,
	Email VARCHAR(50) UNIQUE NOT NULL CHECK(Email LIKE '[a-z,0-9,_,-]%@[a-z,0-9,_,-]%.[a-z][a-z]%'),
	Telemovel INT NOT NULL CHECK(Telemovel LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);
CREATE TABLE CinemasZOZ.Generos_Filme(
	ID_Filme INT IDENTITY(1,1) REFERENCES CinemasZOZ.Filme(ID_Filme),
	ID_Genero INT IDENTITY(1,1) REFERENCES CinemasZOZ.Genero(ID_Genero),
	PRIMARY KEY(ID_Filme, ID_Genero)
);
CREATE TABLE CinemasZOZ.Genero(
	ID_Genero INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(30) UNIQUE NOT NULL
);

CREATE FUNCTION dbo.IsValidURL (@Url VARCHAR(200))
RETURNS INT
AS
BEGIN
    IF CHARINDEX('https://', @url) <> 1
    BEGIN
        RETURN 0;   -- Not a valid URL
    END

    -- Get rid of the http:// stuff
    SET @Url = REPLACE(@URL, 'https://', '');

    -- Now we need to check that we only have digits or numbers
    IF (@Url LIKE '%[^a-zA-Z0-9]%')
    BEGIN
        RETURN 0;
    END

    -- It is a valid URL
    RETURN 1;
END
