CREATE SCHEMA Stock;
GO
CREATE TABLE Stock.PRODUTO(
	Codigo INT,
	Nome VARCHAR(50) NOT NULL,
	Preço SMALLMONEY CHECK (Preço >= 0),
	Taxa_IVA INT,
	Num_Unidades INT,
	PRIMARY KEY (Codigo)
);
CREATE TABLE Stock.TIPO_FORNECEDOR(
	Codigo_Interno INT NOT NULL,
	Designaçao VARCHAR(50) NOT NULL,
	PRIMARY KEY (Codigo_Interno)
);
CREATE TABLE Stock.FORNECEDOR(
	NIF INT NOT NULL,
	Nome VARCHAR(30) NOT NULL,
	Endereço VARCHAR(50) NOT NULL,
	FAX INT NOT NULL,
	Condicoes_Pagamento VARCHAR(20),
	Tipo_Fornecedor INT NOT NULL REFERENCES Stock.TIPO_FORNECEDOR(Codigo_Interno)
	PRIMARY KEY (NIF)
);
CREATE TABLE Stock.ENCOMENDA(
	Num_Encomenda INT NOT NULL,
	Fornecedor INT NOT NULL REFERENCES Stock.FORNECEDOR(NIF),
	Data Date NOT NULL,
	PRIMARY KEY (Num_Encomenda)
);
CREATE TABLE Stock.PRODUTOS_ENCOMENDA(
	Produto INT NOT NULL REFERENCES Stock.PRODUTO(Codigo),
	Encomenda INT REFERENCES Stock.ENCOMENDA(Num_Encomenda),
	Num_Itens INT,
	PRIMARY KEY (Produto, Encomenda)
);
