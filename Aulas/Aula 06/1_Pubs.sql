-- a)
   SELECT * FROM authors;

-- b)
   SELECT au_fname, au_lname, phone FROM authors;

-- c)
   SELECT au_fname, au_lname, phone FROM authors ORDER BY au_fname ASC, au_lname ASC;

-- d)
   SELECT au_fname AS first_name, au_lname AS last_name, phone AS telephone FROM authors ORDER BY au_fname ASC, au_lname ASC;

-- e)
   SELECT au_fname AS first_name, au_lname AS last_name, phone AS telephone FROM authors WHERE state = 'CA' AND au_lname != 'Ringer' ORDER BY au_fname ASC, au_lname ASC;

-- f)
   SELECT * from publishers WHERE pub_name LIKE '%Bo%';

-- g)
   SELECT DISTINCT pub_name from (publishers JOIN titles ON publishers.pub_id = titles.pub_id) WHERE type = 'Business';

-- h)
-- From Sales Table:
       SELECT pub_name, sales FROM publishers JOIN (SELECT publishers.pub_id, SUM(qty) AS sales FROM publishers JOIN titles ON publishers.pub_id = titles.pub_id JOIN sales ON titles.title_id = sales.title_id GROUP BY publishers.pub_id) AS pub_sales ON publishers.pub_id = pub_sales.pub_id;
-- From ytd_sales:
--     SELECT pub_name, SUM(ytd_sales) AS sales FROM ( publishers JOIN titles ON publishers.pub_id = titles.pub_id) GROUP BY pub_name;

-- i)
-- From Sales Table:
       SELECT title, pub_name, sales FROM publishers JOIN titles ON publishers.pub_id = titles.pub_id JOIN (SELECT titles.title_id, SUM(qty) AS sales FROM titles JOIN sales ON titles.title_id = sales.title_id GROUP BY titles.title_id) AS title_sales ON titles.title_id = title_sales.title_id;
-- From ytd_sales:
--     SELECT title, pub_name, SUM(ytd_sales) AS sales FROM ( publishers JOIN titles ON publishers.pub_id = titles.pub_id) GROUP BY pub_name, title;

-- j)
   SELECT title FROM titles JOIN (sales JOIN stores ON sales.stor_id = stores.stor_id) ON titles.title_id = sales.title_id WHERE stor_name = 'Bookbeat';

-- k)
   SELECT DISTINCT au_fname, au_lname FROM titles JOIN (authors JOIN titleauthor ON authors.au_id = titleauthor.au_id) ON titles.title_id = titleauthor.title_id GROUP BY au_fname, au_lname HAVING COUNT(type) > 1;

-- l)
   SELECT type, titles.pub_id, SUM(ytd_sales) AS sales, AVG(price) AS avg_price FROM ((titles JOIN publishers ON titles.pub_id = publishers.pub_id) JOIN sales ON titles.title_id = sales.title_id) GROUP BY type, titles.pub_id;

-- m)
   SELECT type FROM titles GROUP BY type HAVING MAX(advance) > 1.5 * AVG(advance);

-- n)
   SELECT title, au_fname, au_lname, royalty * price / 100 AS money_per_book FROM titles JOIN (titleauthor JOIN authors ON titleauthor.au_id = authors.au_id) ON titles.title_id = titleauthor.title_id;

-- o)
   SELECT ytd_sales AS sales, title, ytd_sales * price AS faturacao, ((royalty*price)/100)*ytd_sales AS author_money, (ytd_sales*price)-(((royalty*price)/100)*ytd_sales) AS publisher_money FROM titles;

-- p)
   SELECT ytd_sales AS sales, title, au_lname + ' ' + au_fname AS author, ytd_sales * price AS faturacao, ((royalty*price)/100)*ytd_sales AS author_money, (ytd_sales*price)-(((royalty*price)/100)*ytd_sales) AS publisher_money FROM titles JOIN (titleauthor JOIN authors ON titleauthor.au_id = authors.au_id) ON titles.title_id = titleauthor.title_id;

-- q)
   SELECT stor_id FROM sales GROUP BY stor_id HAVING COUNT(title_id) = (SELECT COUNT(title_id) FROM titles);

-- r)
   SELECT stor_id FROM sales GROUP BY stor_id HAVING SUM(qty) > (SELECT AVG(stor_qty) FROM (SELECT SUM(qty) AS stor_qty from sales GROUP BY stor_id) AS store);

-- s)
   SELECT title FROM titles LEFT OUTER JOIN (SELECT title_id, sales.stor_id FROM sales JOIN stores ON sales.stor_id = stores.stor_id WHERE stor_name = 'Bookbeat') AS store_sales ON titles.title_id = store_sales.title_id WHERE stor_id IS NULL;

-- t)
   SELECT pub_name, stor_name FROM publishers as p, stores AS s WHERE NOT EXISTS (SELECT * FROM titles JOIN sales ON titles.title_id = sales.title_id WHERE titles.pub_id = p.pub_id AND sales.stor_id = s.stor_id);
