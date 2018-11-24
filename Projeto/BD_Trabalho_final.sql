-- DROP TABLES and SCHEMA:
ALTER TABLE CinemasZOZ.Cinema DROP CONSTRAINT FK_Cinema_Gerente;
ALTER TABLE CinemasZOZ.Cinema DROP CONSTRAINT CK_Cinema_Gerente;
ALTER TABLE CinemasZOZ.Lugar DROP CONSTRAINT CK_Lugar_Sala;

DROP PROC CinemasZOZ.insertCinema;
DROP PROC CinemasZOZ.updateCinema;
DROP PROC CinemasZOZ.removeCinema;

DROP PROC CinemasZOZ.insertFilme;
DROP PROC CinemasZOZ.updateFilme;
DROP PROC CinemasZOZ.removeFilme;

DROP PROC CinemasZOZ.insertDistribuidora;
DROP PROC CinemasZOZ.updateDistribuidora;
DROP PROC CinemasZOZ.removeDistribuidora;

DROP PROC CinemasZOZ.insertTipoBilhete;
DROP PROC CinemasZOZ.updateTipoBilhete;
DROP PROC CinemasZOZ.removeTipoBilhete;

DROP PROC CinemasZOZ.insertEmpregado;
DROP PROC CinemasZOZ.updateEmpregado;
DROP PROC CinemasZOZ.removeEmpregado;
DROP PROC CinemasZOZ.insertOwnEmpregado;
DROP PROC CinemasZOZ.updateOwnEmpregado;
DROP PROC CinemasZOZ.removeOwnEmpregado;

DROP PROC CinemasZOZ.insertSessao;
DROP PROC CinemasZOZ.updateSessao;
DROP PROC CinemasZOZ.removeSessao;
DROP PROC CinemasZOZ.insertOwnSessao;
DROP PROC CinemasZOZ.updateOwnSessao;
DROP PROC CinemasZOZ.removeOwnSessao;

DROP PROC CinemasZOZ.insertLugares;
DROP PROC CinemasZOZ.insertSala;
DROP PROC CinemasZOZ.removeSala;
DROP PROC CinemasZOZ.insertOwnSala;
DROP PROC CinemasZOZ.removeOwnSala;

DROP PROC sp_addempregado;
DROP PROC sp_updateempregado;
DROP PROC sp_removeempregado;

DROP TYPE CinemasZOZ.LugaresListTableType;

DROP FUNCTION CinemasZOZ.getListCinemas;
DROP FUNCTION CinemasZOZ.getListFilmes;
DROP FUNCTION CinemasZOZ.getListDistribuidoras;
DROP FUNCTION CinemasZOZ.getListTiposBilhete;
DROP FUNCTION CinemasZOZ.getListEmpregados;
DROP FUNCTION CinemasZOZ.getOwnListEmpregados;
DROP FUNCTION CinemasZOZ.getListSessoes;
DROP FUNCTION CinemasZOZ.getOwnListSessoes;
DROP FUNCTION CinemasZOZ.getListSalas;
DROP FUNCTION CinemasZOZ.getOwnListSalas;
DROP FUNCTION CinemasZOZ.getListLugaresSala;
DROP FUNCTION CinemasZOZ.getOwnListLugaresSala
DROP FUNCTION CinemasZOZ.getOwnUserRole;
DROP FUNCTION CinemasZOZ.verifyEmpregadoCinema;
DROP FUNCTION CinemasZOZ.verifyLugarSala;

DROP TABLE CinemasZOZ.Bilhetes_Fatura;
DROP TABLE CinemasZOZ.Bilhete;
DROP TABLE CinemasZOZ.TipoBilhete;
DROP TABLE CinemasZOZ.Fatura;
DROP TABLE CinemasZOZ.InstanciaSessao;
DROP TABLE CinemasZOZ.Sessao;
DROP TABLE CinemasZOZ.Filme;
DROP TABLE CinemasZOZ.Distribuidora;
DROP TABLE CinemasZOZ.Lugar;
DROP TABLE CinemasZOZ.Sala;
DROP TABLE CinemasZOZ.Empregado;
DROP TABLE CinemasZOZ.Cinema;

EXEC sp_droprole 'cinemaAdmin';
EXEC sp_droprole 'cinemaGerente';
EXEC sp_droprole 'cinemaEmpregado';

DROP SCHEMA CinemasZOZ;
GO

-- CREATE SCHEMA and TABLES:
CREATE SCHEMA CinemasZOZ;
GO

CREATE TABLE CinemasZOZ.Distribuidora(
	id_dist INT PRIMARY KEY IDENTITY(1,1),
	nome_dist VARCHAR(30) UNIQUE NOT NULL,
	preco_inicial MONEY NOT NULL CHECK(preco_inicial > 0),
	comissao_bilhete SMALLMONEY NOT NULL CHECK(comissao_bilhete >= 0)
);

CREATE TABLE CinemasZOZ.Cinema(
	id_cinema INT PRIMARY KEY IDENTITY(1,1),
	nome_cinema VARCHAR(30) UNIQUE NOT NULL,
	morada VARCHAR(100) NOT NULL,
	telefone INT NOT NULL CHECK(telefone LIKE REPLICATE('[0-9]', 9)),
	gerente INT
);

CREATE TABLE CinemasZOZ.Empregado(
	id_empregado INT PRIMARY KEY IDENTITY(1,1),
	nome_empregado VARCHAR(50) NOT NULL,
	nif INT CHECK(nif LIKE ('[1-9]' + REPLICATE('[0-9]', 8))),
	email VARCHAR(50) UNIQUE NOT NULL CHECK(email LIKE '[a-z,0-9,_,-]%@[a-z,0-9,_,-]%.[a-z][a-z]%'),
	salario SMALLMONEY NOT NULL CHECK(salario > 0),
	cinema INT REFERENCES CinemasZOZ.Cinema(id_cinema)
);

CREATE TABLE CinemasZOZ.Filme(
	id_filme INT PRIMARY KEY IDENTITY(1,1),
	id_dist INT NOT NULL REFERENCES CinemasZOZ.Distribuidora(id_dist),
	titulo VARCHAR(70) UNIQUE NOT NULL,
	idade_min INT NOT NULL CHECK(idade_min >= 6 AND idade_min <= 18),
	duracao INT NOT NULL CHECK(duracao > 0 AND duracao <= 300),
	estreia DATE NOT NULL,
	idioma CHAR(2) NOT NULL CHECK(Idioma LIKE '[A-Z][A-Z]')
);

CREATE TABLE CinemasZOZ.Sala(
	id_cinema INT NOT NULL REFERENCES CINEMASZOZ.Cinema(id_cinema),
	num_sala INT NOT NULL,
	num_filas INT NOT NULL CHECK(num_filas > 0 AND num_filas < 10),
	num_lugares_fila INT NOT NULL CHECK(num_lugares_fila > 0 AND num_lugares_fila < 20),
	PRIMARY KEY (id_cinema, num_sala)
);

CREATE TABLE CinemasZOZ.Sessao(
	id_cinema INT REFERENCES CinemasZOZ.Cinema(ID_Cinema),
	dia_semana SMALLINT NOT NULL CHECK(dia_semana >= 1 AND dia_semana <= 7),
	hora TIME NOT NULL,
	desconto SMALLMONEY CHECK(desconto >= 0),
	PRIMARY KEY(id_cinema, dia_semana, hora)
);

CREATE TABLE CinemasZOZ.TipoBilhete(
	id_tipo_bilhete INT PRIMARY KEY IDENTITY(1,1),
	nome_bilhete VARCHAR(30) UNIQUE NOT NULL,
	restricoes VARCHAR(30) NOT NULL,
	custo SMALLMONEY NOT NULL CHECK(Custo > 0)
);

CREATE TABLE CinemasZOZ.Lugar(
	id_cinema INT NOT NULL,
	num_sala INT NOT NULL,
	fila INT NOT NULL CHECK (fila >= 0),
	num_lugar INT NOT NULL CHECK (num_lugar >= 0),
	normal_deficiente BIT NOT NULL,
	PRIMARY KEY(id_cinema, num_sala, fila, num_lugar),
	FOREIGN KEY (id_cinema, num_sala) REFERENCES CinemasZOZ.Sala(id_cinema, num_sala),
);

CREATE TABLE CinemasZOZ.InstanciaSessao(
	id_cinema INT NOT NULL,
	dia_semana SMALLINT NOT NULL,
	hora TIME NOT NULL,
	dia DATE NOT NULL,
	num_sala INT NOT NULL,
	id_filme INT NOT NULL REFERENCES CinemasZOZ.Filme(id_filme),
	PRIMARY KEY(id_cinema, dia_semana, hora, dia, num_sala),
	FOREIGN KEY (id_cinema, dia_semana, hora) REFERENCES CinemasZOZ.Sessao(id_cinema, dia_semana, hora),
	FOREIGN KEY (id_cinema, num_sala) REFERENCES CinemasZOZ.Sala(id_cinema, num_sala),
	CONSTRAINT UC_Sessao_NumSala UNIQUE (id_cinema, dia_semana, hora, dia, num_sala)
	CHECK (DATEPART(dw, dia) = dia_semana)
);

CREATE TABLE CinemasZOZ.Fatura(
	id_fatura INT PRIMARY KEY IDENTITY(1,1),
	nif_cliente INT CHECK(nif_cliente LIKE ('[1-9]' + REPLICATE('[0-9]', 8))),
	data DATETIME NOT NULL,
	operador INT NOT NULL REFERENCES CinemasZOZ.Empregado(id_empregado)
);

CREATE TABLE CinemasZOZ.Bilhete(
	id_cinema INT NOT NULL,
	dia_semana SMALLINT NOT NULL,
	hora TIME NOT NULL,
	dia DATE NOT NULL,
	num_sala INT NOT NULL,
	fila INT NOT NULL,
	num_lugar INT NOT NULL,
	id_tipo_bilhete INT NOT NULL REFERENCES CinemasZOZ.TipoBilhete(id_tipo_bilhete),
	PRIMARY KEY(id_cinema, dia_semana, hora, dia, num_sala, fila, num_lugar),
	FOREIGN KEY (id_cinema, dia_semana, hora, dia, num_sala) REFERENCES CinemasZOZ.InstanciaSessao(id_cinema, dia_semana, hora, dia, num_sala),
	FOREIGN KEY (id_cinema, num_sala, fila, num_lugar) REFERENCES CinemasZOZ.Lugar(id_cinema, num_sala, fila, num_lugar),
);

CREATE TABLE CinemasZOZ.Bilhetes_Fatura(
	id_fatura INT NOT NULL REFERENCES CinemasZOZ.Fatura(id_fatura),
	id_cinema INT NOT NULL,
	dia_semana SMALLINT NOT NULL,
	hora TIME NOT NULL,
	dia DATE NOT NULL,
	num_sala INT NOT NULL,
	fila INT NOT NULL,
	num_lugar INT NOT NULL,
	PRIMARY KEY(id_fatura, id_cinema, dia_semana, hora, dia, num_sala, fila, num_lugar),
	FOREIGN KEY (id_cinema, dia_semana, hora, dia, num_sala, fila, num_lugar) REFERENCES CinemasZOZ.Bilhete(id_cinema, dia_semana, hora, dia, num_sala, fila, num_lugar),
);
GO

-- Enforce even more CONSTRAINTS:
CREATE FUNCTION CinemasZOZ.verifyEmpregadoCinema(@UserID INT, @CinemaID INT)
RETURNS BIT WITH SCHEMABINDING, ENCRYPTION AS
BEGIN
	IF @UserID IS NULL OR EXISTS(SELECT id_empregado, cinema FROM CinemasZOZ.Empregado WHERE id_empregado = @UserID AND cinema = @CinemaID)
		RETURN 1;
	RETURN 0;
END;
GO

CREATE FUNCTION CinemasZOZ.verifyLugarSala(@CinemaID INT, @NumSala INT, @Fila INT, @NumLugar INT)
RETURNS BIT WITH SCHEMABINDING, ENCRYPTION AS
BEGIN
	DECLARE @NumFilas INT;
	DECLARE @NumLugaresFila INT;
	SELECT @NumFilas = num_filas, @NumLugaresFila = num_lugares_fila FROM CinemasZOZ.Sala WHERE id_cinema = @CinemaID AND num_sala = @NumSala;
	IF @Fila < @NumFilas AND @NumLugar < @NumLugaresFila
		RETURN 1;
	RETURN 0;
END;
GO

ALTER TABLE CinemasZOZ.Cinema ADD CONSTRAINT FK_Cinema_Gerente FOREIGN KEY (gerente) REFERENCES CinemasZOZ.Empregado(id_empregado);
ALTER TABLE CinemasZOZ.Cinema ADD CONSTRAINT CK_Cinema_Gerente CHECK(CinemasZOZ.verifyEmpregadoCinema(gerente, id_cinema) = 1);
ALTER TABLE CinemasZOZ.Lugar ADD CONSTRAINT CK_Lugar_Sala CHECK(CinemasZOZ.verifyLugarSala(id_cinema, num_sala, fila, num_lugar) = 1);
GO

-- CREATE ROLES
EXEC sp_addrole 'cinemaAdmin';
EXEC sp_addrole 'cinemaGerente';
EXEC sp_addrole 'cinemaEmpregado';
GO

-- CREATE TRIGGERS
/*
CREATE TRIGGER tg_addFuncionarioPermissions ON CinemasZOZ.Empregado AFTER INSERT
AS
	DECLARE @IdEmpregado INT;

	DECLARE c CURSOR FAST_FORWARD
	FOR SELECT id_empregado FROM inserted;

	OPEN c;
	FETCH c INTO @IdEmpregado;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		

		FETCH c INTO @IdEmpregado;
	END;
	
	CLOSE c;
	DEALLOCATE c;

GO
*/

-- CREATE PROCEDURES and FUNCTIONS:
CREATE PROC sp_addempregado(@IdEmpregado INT, @Password VARCHAR(128), @IsAdmin BIT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
	DECLARE @LoginName sysname = 'ZOZ' + CAST(@IdEmpregado AS VARCHAR(10));
	EXEC sp_addlogin @LoginName, @Password;
	EXEC sp_adduser @LoginName, @LoginName;
	EXEC sp_addrolemember 'cinemaEmpregado', @LoginName;

	IF (@IsAdmin = 1)
		EXEC sp_addrolemember 'cinemaAdmin', @LoginName;
END;
GO

CREATE PROC sp_updateempregado(@IdEmpregado INT, @IsAdmin BIT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
	DECLARE @LoginName sysname = 'ZOZ' + CAST(@IdEmpregado AS VARCHAR(10));
	IF (@IsAdmin = 1)
		EXEC sp_addrolemember 'cinemaAdmin', @LoginName;
	ELSE
		EXEC sp_droprolemember 'cinemaAdmin', @LoginName;
END;
GO

CREATE PROC sp_removeempregado(@IdEmpregado INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
	DECLARE @LoginName sysname = 'ZOZ' + CAST(@IdEmpregado AS VARCHAR(10));
	EXEC sp_dropuser @LoginName;
	EXEC sp_droplogin @LoginName;
END;
GO

CREATE FUNCTION CinemasZOZ.getOwnUserRole()
RETURNS INT WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS CALLER AS
BEGIN
	IF (IS_ROLEMEMBER('cinemaAdmin') = 1 OR IS_ROLEMEMBER('db_owner') = 1)
		RETURN 0;
	IF (IS_ROLEMEMBER('cinemaGerente') = 1)
		RETURN 1;
	IF (IS_ROLEMEMBER('cinemaEmpregado') = 1)
		RETURN 2;
	RETURN -1;
END;
GO
GRANT EXECUTE ON CinemasZOZ.getOwnUserRole TO cinemaEmpregado;
GO

CREATE FUNCTION CinemasZOZ.getListCinemas ()
RETURNS TABLE
WITH SCHEMABINDING, ENCRYPTION AS
	RETURN SELECT id_cinema, nome_cinema, morada, telefone, gerente FROM CinemasZOZ.Cinema;
GO
GRANT SELECT ON CinemasZOZ.getListCinemas TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertCinema (@NomeCinema VARCHAR(30), @Morada VARCHAR(100), @Telefone INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT INTO CinemasZOZ.Cinema (nome_cinema, morada, telefone) VALUES (@NomeCinema, @Morada, @Telefone);
	RETURN SCOPE_IDENTITY();
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertCinema TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.updateCinema (@IdCinema INT, @NomeCinema VARCHAR(30), @Morada VARCHAR(100), @Telefone INT, @Gerente INT = NULL)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	UPDATE CinemasZOZ.Cinema SET nome_cinema = @NomeCinema, morada = @Morada, telefone = @Telefone, gerente = @Gerente WHERE id_cinema = @IdCinema;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateCinema TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeCinema (@IdCinema INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DELETE FROM CinemasZOZ.Cinema WHERE id_cinema = @IdCinema;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeCinema TO cinemaAdmin;
GO
GRANT EXECUTE ON CinemasZOZ.removeCinema TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getListFilmes ()
RETURNS @table TABLE (id_filme INT, id_dist INT, titulo VARCHAR(70), idade_min INT, duracao INT, estreia DATE, idioma CHAR(2))
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_filme, id_dist, titulo, idade_min, duracao, estreia, idioma FROM CinemasZOZ.Filme ORDER BY titulo;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListFilmes TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertFilme (@IdDist INT, @Titulo VARCHAR(70), @Idade INT, @Duracao INT, @Estreia DATE, @Idioma CHAR(2))
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdFilme INT;
	SET XACT_ABORT ON;
	BEGIN TRAN
		INSERT INTO CinemasZOZ.Filme (id_dist, titulo, idade_min, duracao, estreia, idioma) VALUES (@IdDist, @Titulo, @Idade, @Duracao, @Estreia, @Idioma);
		SET @IdFilme = SCOPE_IDENTITY();
	COMMIT TRAN
	RETURN @IdFilme;
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertFilme TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.updateFilme (@IdFilme INT, @IdDist INT, @Titulo VARCHAR(70), @Idade INT, @Duracao INT, @Estreia DATE, @Idioma CHAR(2))
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN
		UPDATE CinemasZOZ.Filme SET id_dist = @IdDist, titulo = @Titulo, idade_min = @Idade, duracao = @Duracao, estreia = @Estreia, idioma = @Idioma WHERE id_filme = @IdFilme;
	COMMIT TRAN
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateFilme TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeFilme (@IdFilme INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN
		DELETE FROM CinemasZOZ.Filme WHERE id_filme = @IdFilme;
	COMMIT TRAN
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeFilme TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getListDistribuidoras ()
RETURNS @table TABLE (id_dist INT, nome_dist VARCHAR(30), preco_inicial MONEY, comissao_bilhete SMALLMONEY)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_dist, nome_dist, preco_inicial, comissao_bilhete FROM CinemasZOZ.Distribuidora;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListDistribuidoras TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertDistribuidora (@NomeDist VARCHAR(30), @PrecoInicial MONEY, @ComissaoBilhete SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT INTO CinemasZOZ.Distribuidora(nome_dist, preco_inicial, comissao_bilhete) VALUES (@NomeDist, @PrecoInicial, @ComissaoBilhete);
	RETURN SCOPE_IDENTITY();
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertDistribuidora TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.updateDistribuidora (@IdDist INT, @NomeDist VARCHAR(30), @PrecoInicial MONEY, @ComissaoBilhete SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	UPDATE CinemasZOZ.Distribuidora SET nome_dist = @NomeDist, preco_inicial = @PrecoInicial, comissao_bilhete = @ComissaoBilhete WHERE id_dist = @IdDist;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateDistribuidora TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeDistribuidora (@IdDist INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DELETE FROM CinemasZOZ.Distribuidora WHERE id_dist = @IdDist;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeDistribuidora TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getListTiposBilhete ()
RETURNS @table TABLE (id_tipo_bilhete INT, nome_bilhete VARCHAR(30), restricoes VARCHAR(30), custo SMALLMONEY)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_tipo_bilhete, nome_bilhete, restricoes, custo FROM CinemasZOZ.TipoBilhete;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListTiposBilhete TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertTipoBilhete (@NomeBilhete VARCHAR(30), @Restricoes VARCHAR(30), @Custo SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT INTO CinemasZOZ.TipoBilhete(nome_bilhete, restricoes, custo) VALUES (@NomeBilhete, @Restricoes, @Custo);
	RETURN SCOPE_IDENTITY();
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertTipoBilhete TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.updateTipoBilhete (@IdTipoBilhete INT, @NomeBilhete VARCHAR(30), @Restricoes VARCHAR(30), @Custo SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	UPDATE CinemasZOZ.TipoBilhete SET nome_bilhete = @NomeBilhete, restricoes = @Restricoes, custo = @Custo WHERE id_tipo_bilhete = @IdTipoBilhete;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateTipoBilhete TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeTipoBilhete (@IdTipoBilhete INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DELETE FROM CinemasZOZ.TipoBilhete WHERE id_tipo_bilhete = @IdTipoBilhete;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeTipoBilhete TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getListEmpregados (@IdCinema INT)
RETURNS @table TABLE (id_empregado INT, nome_empregado VARCHAR(50), nif INT, email VARCHAR(50), salario SMALLMONEY, cinema INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_empregado, nome_empregado, nif, email, salario, cinema FROM CinemasZOZ.Empregado WHERE cinema = @IdCinema;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListEmpregados TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListEmpregados ()
RETURNS @table TABLE (id_empregado INT, nome_empregado VARCHAR(50), nif INT, email VARCHAR(50), salario SMALLMONEY, cinema INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT id_empregado, nome_empregado, nif, email, salario, cinema FROM CinemasZOZ.Empregado WHERE cinema = @IdCinema;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListEmpregados TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.insertEmpregado (@NomeEmpregado VARCHAR(50), @Password VARCHAR(128), @Nif INT, @Email VARCHAR(50), @Salario SMALLMONEY, @IdCinema INT = NULL)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdEmpregado INT;
	DECLARE @IsAdmin BIT = (@IdCinema IS NULL);

	INSERT INTO CinemasZOZ.Empregado(nome_empregado, nif, email, salario, cinema) VALUES (@NomeEmpregado, @Nif, @Email, @Salario, @IdCinema);
	SET @IdEmpregado = SCOPE_IDENTITY();

	EXEC sp_addempregado @IdEmpregado, @Password, @IsAdmin;

	RETURN @IdEmpregado;
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertEmpregado TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertOwnEmpregado (@NomeEmpregado VARCHAR(50), @Password VARCHAR(128), @Nif INT, @Email VARCHAR(50), @Salario SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdEmpregado INT;
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));

	INSERT INTO CinemasZOZ.Empregado(nome_empregado, nif, email, salario, cinema) VALUES (@NomeEmpregado, @Nif, @Email, @Salario, @IdCinema);
	SET @IdEmpregado = SCOPE_IDENTITY();
	
	EXEC sp_addempregado @IdEmpregado, @Password, 0;

	RETURN @IdEmpregado;
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertOwnEmpregado TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.updateEmpregado (@IdEmpregado INT, @NomeEmpregado VARCHAR(50), @Nif INT, @Email VARCHAR(50), @Salario SMALLMONEY, @IdCinema INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	UPDATE CinemasZOZ.Empregado SET nome_empregado = @NomeEmpregado, nif = @Nif, email = @Email, salario = @Salario, cinema = @IdCinema WHERE id_empregado = @IdEmpregado;

	DECLARE @IsAdmin BIT = (@IdCinema IS NULL);
	EXEC sp_updateempregado @IdEmpregado, @IsAdmin;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateEmpregado TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.updateOwnEmpregado (@IdEmpregado INT, @NomeEmpregado VARCHAR(50), @Nif INT, @Email VARCHAR(50), @Salario SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	UPDATE CinemasZOZ.Empregado SET nome_empregado = @NomeEmpregado, nif = @Nif, email = @Email, salario = @Salario WHERE id_empregado = @IdEmpregado AND cinema = @IdCinema;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateOwnEmpregado TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.removeEmpregado (@IdEmpregado INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DELETE FROM CinemasZOZ.Empregado WHERE id_empregado = @IdEmpregado;
	EXEC sp_removeempregado @IdEmpregado;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeEmpregado TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeOwnEmpregado (@IdEmpregado INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	DELETE FROM CinemasZOZ.Empregado WHERE id_empregado = @IdEmpregado AND cinema = @IdCinema;
	EXEC sp_removeempregado @IdEmpregado;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeOwnEmpregado TO cinemaGerente;
GO

CREATE FUNCTION CinemasZOZ.getListSessoes (@IdCinema INT, @DiaSemana SMALLINT)
RETURNS @table TABLE (id_cinema INT, dia_semana SMALLINT, hora TIME, desconto SMALLMONEY)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_cinema, dia_semana, hora, desconto FROM CinemasZOZ.Sessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListSessoes TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListSessoes (@DiaSemana SMALLINT)
RETURNS @table TABLE (id_cinema INT, dia_semana SMALLINT, hora TIME, desconto SMALLMONEY)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT id_cinema, dia_semana, hora, desconto FROM CinemasZOZ.Sessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListSessoes TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.insertSessao (@DiaSemana SMALLINT, @Hora TIME, @Desconto SMALLMONEY, @IdCinema INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT INTO CinemasZOZ.Sessao(id_cinema, dia_semana, hora, desconto) VALUES (@IdCinema, @DiaSemana, @Hora, @Desconto);
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertSessao TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertOwnSessao (@DiaSemana SMALLINT, @Hora TIME, @Desconto SMALLMONEY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT INTO CinemasZOZ.Sessao(id_cinema, dia_semana, hora, desconto) VALUES (@IdCinema, @DiaSemana, @Hora, @Desconto);
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertOwnSessao TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.updateSessao (@DiaSemana SMALLINT, @Hora TIME, @Desconto SMALLMONEY, @IdCinema INT, @NewDiaSemana SMALLINT = NULL, @NewHora TIME = NULL)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	IF @NewDiaSemana IS NULL
		SET @NewDiaSemana = @DiaSemana;
	IF @NewHora IS NULL
		SET @NewHora = @Hora;
	UPDATE CinemasZOZ.Sessao SET dia_semana = @NewDiaSemana, hora = @NewHora, desconto = @Desconto WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateSessao TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.updateOwnSessao (@DiaSemana SMALLINT, @Hora TIME, @Desconto SMALLMONEY, @NewDiaSemana SMALLINT = NULL, @NewHora TIME = NULL)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	IF @NewDiaSemana IS NULL
		SET @NewDiaSemana = @DiaSemana;
	IF @NewHora IS NULL
		SET @NewHora = @Hora;
	UPDATE CinemasZOZ.Sessao SET dia_semana = @NewDiaSemana, hora = @NewHora, desconto = @Desconto WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora;
END;
GO
GRANT EXECUTE ON CinemasZOZ.updateSessao TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.removeSessao (@DiaSemana SMALLINT, @Hora TIME, @IdCinema INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DELETE FROM CinemasZOZ.Sessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeSessao TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeOwnSessao (@DiaSemana SMALLINT, @Hora TIME)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	DELETE FROM CinemasZOZ.Sessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeOwnSessao TO cinemaGerente;
GO

CREATE FUNCTION CinemasZOZ.getListSalas (@IdCinema INT)
RETURNS @table TABLE (id_cinema INT, num_sala INT, num_filas INT, num_lugares_fila INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_cinema, num_sala, num_filas, num_lugares_fila FROM CinemasZOZ.Sala WHERE id_cinema = @IdCinema;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListSalas TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListSalas ()
RETURNS @table TABLE (id_cinema INT, num_sala INT, num_filas INT, num_lugares_fila INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT id_cinema, num_sala, num_filas, num_lugares_fila FROM CinemasZOZ.Sala WHERE id_cinema = @IdCinema;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListSalas TO cinemaGerente;
GO

CREATE FUNCTION CinemasZOZ.getListLugaresSala (@IdCinema INT, @NumSala INT)
RETURNS @table TABLE (id_cinema INT, num_sala INT, fila INT, num_lugar INT, normal_deficiente BIT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_cinema, num_sala, fila, num_lugar, normal_deficiente FROM CinemasZOZ.Lugar WHERE id_cinema = @IdCinema AND num_sala = @NumSala;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListLugaresSala TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListLugaresSala ( @NumSala INT)
RETURNS @table TABLE (id_cinema INT, num_sala INT, fila INT, num_lugar INT, normal_deficiente BIT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT id_cinema, num_sala, fila, num_lugar, normal_deficiente FROM CinemasZOZ.Lugar WHERE id_cinema = @IdCinema AND num_sala = @NumSala;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListLugaresSala TO cinemaGerente;
GO

CREATE TYPE CinemasZOZ.LugaresListTableType AS TABLE (fila INT, num_lugar INT, lugar_def BIT);
GO
GRANT EXECUTE ON TYPE :: CinemasZOZ.LugaresListTableType TO cinemaAdmin, cinemaGerente;
GO

CREATE PROC CinemasZOZ.insertLugares (@IdCinema INT, @numSala INT, @Lugares CinemasZOZ.LugaresListTableType READONLY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT INTO CinemasZOZ.Lugar(id_cinema, num_sala, fila, num_lugar, normal_deficiente) SELECT @IdCinema, @numSala, fila, num_lugar, lugar_def FROM @Lugares;
END;
GO

CREATE PROC CinemasZOZ.insertSala (@numSala INT, @NumFilas INT, @NumLugaresFila INT, @Lugares CinemasZOZ.LugaresListTableType READONLY, @IdCinema INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	BEGIN TRAN
		INSERT INTO CinemasZOZ.Sala(num_sala, id_cinema, num_filas, num_lugares_fila) VALUES (@numSala, @IdCinema, @NumFilas, @NumLugaresFila)
		EXEC CinemasZOZ.insertLugares @IdCinema, @numSala, @Lugares;
	COMMIT TRAN
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertSala TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertOwnSala (@numSala INT, @NumFilas INT, @NumLugaresFila INT, @Lugares CinemasZOZ.LugaresListTableType READONLY)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	BEGIN TRAN
		DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
		INSERT INTO CinemasZOZ.Sala(num_sala, id_cinema, num_filas, num_lugares_fila) VALUES (@numSala, @IdCinema, @NumFilas, @NumLugaresFila)
		EXEC CinemasZOZ.insertLugares @IdCinema, @numSala, @Lugares;
	COMMIT TRAN
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertOwnSala TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.removeSala (@UserId INT, @Password VARCHAR(128), @NumSala INT, @IdCinema INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	BEGIN TRAN
		DELETE FROM CinemasZOZ.Lugar WHERE num_sala = @NumSala AND id_cinema = @IdCinema;
		DELETE FROM CinemasZOZ.Sala WHERE num_sala = @NumSala AND id_cinema = @IdCinema;
	COMMIT TRAN
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeSala TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeOwnSala (@UserId INT, @Password VARCHAR(128), @NumSala INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	BEGIN TRAN
		DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
		DELETE FROM CinemasZOZ.Lugar WHERE num_sala = @NumSala AND id_cinema = @IdCinema;
		DELETE FROM CinemasZOZ.Sala WHERE num_sala = @NumSala AND id_cinema = @IdCinema;
	COMMIT TRAN
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeOwnSala TO cinemaGerente;
GO

CREATE FUNCTION CinemasZOZ.getListInstSessoes (@IdCinema INT, @DiaSemana SMALLINT, @Hora TIME)
RETURNS @table TABLE (id_cinema INT, dia_semana SMALLINT, hora TIME, dia DATE, num_sala INT, id_filme INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_cinema, dia_semana, hora, dia, num_sala, id_filme FROM CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListInstSessoes TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListInstSessoes (@DiaSemana SMALLINT, @Hora TIME)
RETURNS @table TABLE (id_cinema INT, dia_semana SMALLINT, hora TIME, dia DATE, num_sala INT, id_filme INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT id_cinema, dia_semana, hora, dia, num_sala, id_filme FROM CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListInstSessoes TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.insertInstSessao (@IdCinema INT, @DiaSemana SMALLINT, @Hora TIME, @Dia DATE, @NumSala INT, @IdFilme INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT INTO CinemasZOZ.InstanciaSessao(id_cinema, dia_semana, hora, dia, num_sala, id_filme) VALUES (@IdCinema, @DiaSemana, @Hora, @Dia, @NumSala, @IdFilme);
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertInstSessao TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.insertOwnInstSessao (@DiaSemana SMALLINT, @Hora TIME, @Dia DATE, @NumSala INT, @IdFilme INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT INTO CinemasZOZ.InstanciaSessao(id_cinema, dia_semana, hora, dia, num_sala, id_filme) VALUES (@IdCinema, @DiaSemana, @Hora, @Dia, @NumSala, @IdFilme);
END;
GO
GRANT EXECUTE ON CinemasZOZ.insertOwnInstSessao TO cinemaGerente;
GO

CREATE PROC CinemasZOZ.removeInstSessao (@IdCinema INT, @DiaSemana SMALLINT, @Hora TIME, @Dia DATE)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DELETE FROM CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora AND dia = @Dia;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeInstSessao TO cinemaAdmin;
GO

CREATE PROC CinemasZOZ.removeOwnInstSessao (@DiaSemana SMALLINT, @Hora TIME, @Dia DATE)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	DELETE FROM CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @Hora AND dia = @Dia;
END;
GO
GRANT EXECUTE ON CinemasZOZ.removeOwnInstSessao TO cinemaGerente;
GO

CREATE FUNCTION CinemasZOZ.getListFilmesPorDia (@IdCinema INT, @Dia DATE)
RETURNS @table TABLE (id_filme INT, id_dist INT, titulo VARCHAR(70), idade_min INT, duracao INT, estreia DATE, idioma CHAR(2))
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT DISTINCT id_filme, id_dist, titulo, idade_min, duracao, estreia, idioma FROM CinemasZOZ.Filme JOIN CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia = @Dia;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListFilmesPorDia TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListFilmesPorDia (@Dia DATE)
RETURNS @table TABLE (id_filme INT, id_dist INT, titulo VARCHAR(70), idade_min INT, duracao INT, estreia DATE, idioma CHAR(2))
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT DISTINCT id_filme, id_dist, titulo, idade_min, duracao, estreia, idioma FROM CinemasZOZ.Filme JOIN CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia = @Dia;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListFilmesPorDia TO cinemaGerente, cinemaEmpregado;
GO

CREATE FUNCTION CinemasZOZ.getListInstSessoesPorDiaFilme (@IdCinema INT, @Dia DATE, @IdFilme INT)
RETURNS @table TABLE (id_cinema INT, dia_semana SMALLINT, hora TIME, dia DATE, num_sala INT, id_filme INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT id_cinema, dia_semana, hora, dia, num_sala, id_filme FROM CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia = @Dia AND id_filme = @IdFilme;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListInstSessoesPorDiaFilme TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListInstSessoesPorDiaFilme (@Dia DATE, @IdFilme INT)
RETURNS @table TABLE (id_cinema INT, dia_semana SMALLINT, hora TIME, dia DATE, num_sala INT, id_filme INT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT id_cinema, dia_semana, hora, dia, num_sala, id_filme FROM CinemasZOZ.InstanciaSessao WHERE id_cinema = @IdCinema AND dia = @Dia AND id_filme = @IdFilme;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListInstSessoesPorDiaFilme TO cinemaGerente, cinemaEmpregado;
GO

CREATE FUNCTION CinemasZOZ.getListLugaresInstSessao (@IdCinema INT, @DiaSemana SMALLINT, @Hora TIME, @Dia DATE, @NumSala INT)
RETURNS @table TABLE (id_cinema INT, num_sala INT, fila INT, num_lugar INT, ocupado BIT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	INSERT @table SELECT @IdCinema, @NumSala, fila, num_lugar, 1 FROM CinemasZOZ.Bilhetes WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @HORA AND dia = @Dia AND sala = @NumSala;
	INSERT @table SELECT @IdCinema, @NumSala, fila, num_lugar, 0 FROM @table AS t RIGHT OUTER JOIN CinemasZOZ.Lugar AS l
		ON t.id_cinema = l.id_cinema AND t.num_sala = l.num_sala AND t.fila = l.fila AND t.num_lugar = l.num_lugar
		WHERE id_cinema = @IdCinema AND num_sala = @NumSala AND t.ocupado IS NULL;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getListLugaresInstSessao TO cinemaAdmin;
GO

CREATE FUNCTION CinemasZOZ.getOwnListLugaresInstSessao (@DiaSemana SMALLINT, @Hora TIME, @Dia DATE, @NumSala INT)
RETURNS @table TABLE (id_cinema INT, num_sala INT, fila INT, num_lugar INT, ocupado BIT)
WITH SCHEMABINDING, ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	DECLARE @IdCinema INT = (SELECT cinema FROM CinemasZOZ.Empregado WHERE id_empregado = SUBSTRING(ORIGINAL_LOGIN(), 4, LEN(ORIGINAL_LOGIN())));
	INSERT @table SELECT @IdCinema, @NumSala, fila, num_lugar, 1 FROM CinemasZOZ.Bilhetes WHERE id_cinema = @IdCinema AND dia_semana = @DiaSemana AND hora = @HORA AND dia = @Dia AND sala = @NumSala;
	INSERT @table SELECT @IdCinema, @NumSala, fila, num_lugar, 0 FROM @table AS t RIGHT OUTER JOIN CinemasZOZ.Lugar AS l
		ON t.id_cinema = l.id_cinema AND t.num_sala = l.num_sala AND t.fila = l.fila AND t.num_lugar = l.num_lugar
		WHERE id_cinema = @IdCinema AND num_sala = @NumSala AND t.ocupado IS NULL;
	RETURN;
END;
GO
GRANT SELECT ON CinemasZOZ.getOwnListLugaresInstSessao TO cinemaGerente, cinemaEmpregado;
GO

CREATE TYPE CinemasZOZ.LugaresBilheteListTableType AS TABLE (fila INT, num_lugar INT, id_tipo_bilhete INT);
GO
GRANT EXECUTE ON TYPE :: CinemasZOZ.LugaresListTableType TO cinemaAdmin, cinemaGerente, cinemaEmpregado;
GO

CREATE PROC CinemasZOZ.insertBilhetes (@IdCinema INT, @DiaSemana SMALLINT, @Hora TIME, @Dia DATE, @NumSala INT, @Lugares CinemasZOZ.LugaresBilheteListTableType READONLY, @Nif INT)
WITH ENCRYPTION, EXECUTE AS OWNER AS
BEGIN
	BEGIN TRAN
	 	DECLARE @IdFatura INT;
		INSERT INTO CinemasZOZ.Bilhete SELECT @IdCinema, @DiaSemana, @Hora, @Dia, @NumSala, fila, num_lugar, id_tipo_bilhete FROM @Lugares;
		INSERT INTO CinemasZOZ.Fatura (nif_cliente, data, operador) VALUES (@Nif, GETDATE(), 0)
		SET @IdFatura = SCOPE_IDENTITY();
		INSERT INTO CinemasZOZ.Bilhetes_Fatura SELECT @IdFatura, @IdCinema, @DiaSemana, @Hora, @Dia, @NumSala, fila, num_lugar FROM @Lugares;
	COMMIT TRAN
END;
GO

-- CREATE First User
/*
EXEC sp_addlogin 'admin', 'admin', 'p4g9';
CREATE USER admin FOR LOGIN admin;
EXEC sp_addrolemember 'cinemaEmpregado', 'admin';
EXEC sp_addrolemember 'cinemaAdmin', 'admin';*/

/*
EXEC sp_addlogin 'ZOZ1', 'ZOZ1', 'p4g9';
CREATE USER ZOZ1 FOR LOGIN ZOZ1;
EXEC sp_addrolemember 'cinemaEmpregado', 'ZOZ1';*/