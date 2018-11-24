-- a)
--   i.
--   CREATE VIEW View1_Titulo_Autor AS SELECT title, au_fname + ' ' + au_lname AS author_name FROM ((Pubs.dbo.titles AS t JOIN Pubs.dbo.titleauthor AS ta ON t.title_id = ta.title_id) JOIN Pubs.dbo.authors AS a ON ta.au_id = a.au_id);
--   ii.
--   CREATE VIEW View2_Editor_Funcionario AS SELECT pub_name, fname + ' ' + minit + ' '+ lname AS func_name FROM (Pubs.dbo.publishers AS p JOIN Pubs.dbo.employee AS e ON p.pub_id = e.pub_id);
--   iii.
--   CREATE VIEW View3_Loja_Titulo AS SELECT stor_name, title FROM ((Pubs.dbo.stores AS st JOIN Pubs.dbo.sales AS s ON st.stor_id = s.stor_id) JOIN Pubs.dbo.titles AS t ON s.title_id = t.title_id);
--   iv.
--   CREATE VIEW View4_Livros_Business AS SELECT title FROM Pubs.dbo.titles WHERE type = 'Business';

-- b)
--   i.
--   SELECT * FROM View1_Titulo_Autor;
--   ii.
--   SELECT * FROM View2_Editor_Funcionario;
--   iii.
--   SELECT * FROM View3_Loja_Titulo;
--   iv.
--   SELECT * FROM View4_Livros_Business;

-- c)
-- ALTER VIEW View1_Titulo_Autor AS SELECT t.title_id, title, au_fname + ' ' + au_lname AS author_name FROM ((Pubs.dbo.titles AS t JOIN Pubs.dbo.titleauthor AS ta ON t.title_id = ta.title_id) JOIN Pubs.dbo.authors AS a ON ta.au_id = a.au_id);
-- ALTER VIEW View3_Loja_Titulo AS SELECT stor_name, t.title_id, title FROM ((Pubs.dbo.stores AS st JOIN Pubs.dbo.sales AS s ON st.stor_id = s.stor_id) JOIN Pubs.dbo.titles AS t ON s.title_id = t.title_id);
-- SELECT stor_name, author_name FROM (View1_Titulo_Autor AS ta JOIN View3_Loja_Titulo AS lt ON ta.title_id = lt.title_id) ORDER BY stor_name, author_name; 

-- d)
-- Não faz sentido pois a view permite inserir livros de tipos diferentes de Business
-- ALTER VIEW View4_Livros_Business AS SELECT * FROM Pubs.dbo.titles WHERE type = 'Business' WITH CHECK OPTION;
/*
   insert into View4_Livros_Business (title_id, title, type, pub_id, price, notes)
   values('BDTst1', 'New BD Book','popular_comp', '1389', $30.00, 'A must-read for DB course.')
*/