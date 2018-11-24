using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD___Project.Entities;
using System.Data;

namespace BD___Project.Helpers
{
    public static class DatabaseHelper
    {
        private static string dataSource = "localhost";
        private static string initialCatalog = "p4g9";

        private static SqlConnection cn;
        private static string userID;
        private static string password;

        public static void setLogin(string userID, string password)
        {
            DatabaseHelper.userID = userID;
            DatabaseHelper.password = password;

            if (cn != null && cn.State != System.Data.ConnectionState.Closed)
                cn.Close();
            cn = null;
        }

        private static void setupSGBDConnection()
        {
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException();
            if (cn == null)
            {
                SqlConnectionStringBuilder cnStrBuilder = new SqlConnectionStringBuilder();
                cnStrBuilder.DataSource = dataSource;
                cnStrBuilder.InitialCatalog = initialCatalog;
                cnStrBuilder.UserID = userID;
                cnStrBuilder.Password = password;

                cn = new SqlConnection(cnStrBuilder.ConnectionString);
            }
            if (cn.State != System.Data.ConnectionState.Open)
                cn.Open();
        }

        private static bool verifySGBDConnection()
        {
            try
            {
                setupSGBDConnection();
                return cn.State == System.Data.ConnectionState.Open;
            }
            catch (SqlException e)
            {
                Debug.Print("SGBD Connection failed: " + e.Message);
                return false;
            }
        }

        public static int getOwnUserRole()
        {
            if (!verifySGBDConnection())
                return -2;
            else
            {
                //SqlCommand cmd = new SqlCommand("SELECT * FROM ")
                SqlCommand cmd = new SqlCommand("SELECT CinemasZOZ.getOwnUserRole();", cn);
                // TODO: getUserRole
                int userRole = (int) cmd.ExecuteScalar();

                return userRole;
            }
        }
        
        public static class AdminCommands
        {
            public static List<Cinema> getCinemaList()
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListCinemas()", cn);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cinema> list = new List<Cinema>();

                    while (reader.Read())
                    {
                        Cinema cinema = new Cinema();
                        cinema.id = int.Parse(reader["id_cinema"].ToString());
                        cinema.Title = reader["nome_cinema"].ToString();
                        cinema.morada = reader["morada"].ToString();
                        cinema.telefone = reader["telefone"].ToString();
                        cinema.gerente = reader["gerente"] as int? ?? -1;

                        list.Add(cinema);
                    }
                    cn.Close();
                    return list;
                } catch(Exception e)
                {
                    Debug.Print("Get cinema list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static Cinema insertCinema(string nome, string morada, string telefone)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertCinema", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NomeCinema", nome);
                cmd.Parameters.AddWithValue("@Morada", morada);
                cmd.Parameters.AddWithValue("@Telefone", telefone);
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int) cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new Cinema(id, nome, morada, telefone, -1);
                }catch (Exception e)
                {
                    Debug.Print("Insert cinema failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static bool updateCinema(Cinema c)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateCinema", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", c.id);
                cmd.Parameters.AddWithValue("@NomeCinema", c.Title);
                cmd.Parameters.AddWithValue("@Morada", c.morada);
                cmd.Parameters.AddWithValue("@Telefone", c.telefone);
                if (c.gerente != -1)
                    cmd.Parameters.AddWithValue("@Gerente", c.gerente);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Update cinema failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeCinema(Cinema c)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeCinema", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", c.id);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Remove cinema failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static List<Filme> getFilmeList()
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListFilmes()", cn);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Filme> list = new List<Filme>();

                    while (reader.Read())
                    {
                        Filme filme = new Filme();
                        filme.id = int.Parse(reader["id_filme"].ToString());
                        filme.dist = int.Parse(reader["id_dist"].ToString());
                        filme.Title = reader["titulo"].ToString();
                        filme.idadeMin = int.Parse(reader["idade_min"].ToString());
                        filme.duracao = int.Parse(reader["duracao"].ToString());
                        filme.estreia = reader["estreia"].ToString();
                        filme.idioma = reader["idioma"].ToString();

                        list.Add(filme);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get filme list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static Filme insertFilme(int idDist, string titulo, int idadeMin, int duracao, string estreia, string idioma)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertFilme", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdDist", idDist);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Idade", idadeMin);
                cmd.Parameters.AddWithValue("@Duracao", duracao);
                cmd.Parameters.AddWithValue("@Estreia", estreia);
                cmd.Parameters.AddWithValue("@Idioma", idioma);
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new Filme(id, idDist, titulo, idadeMin, duracao, estreia, idioma);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert filme failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static bool updateFilme(Filme f)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateFilme", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdFilme", f.id);
                cmd.Parameters.AddWithValue("@IdDist", f.dist);
                cmd.Parameters.AddWithValue("@Titulo", f.Title);
                cmd.Parameters.AddWithValue("@Idade", f.idadeMin);
                cmd.Parameters.AddWithValue("@Duracao", f.duracao);
                cmd.Parameters.AddWithValue("@Estreia", f.estreia);
                cmd.Parameters.AddWithValue("@Idioma", f.idioma);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Update filme failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeFilme(Filme f)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeFilme", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdFilme", f.id);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Remove filme failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static List<Empregado> getEmpregadoList(int cinemaId)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListEmpregados(@IdCinema)", cn);
                cmd.Parameters.AddWithValue("@IdCinema", cinemaId);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Empregado> list = new List<Empregado>();

                    while (reader.Read())
                    {
                        Empregado empregado = new Empregado();
                        empregado.id = int.Parse(reader["id_empregado"].ToString());
                        empregado.Title = reader["nome_empregado"].ToString();
                        empregado.nif = reader["nif"].ToString();
                        empregado.email = reader["email"].ToString();
                        empregado.salario = reader["salario"].ToString();
                        empregado.cinema = reader["cinema"] as int? ?? -1;

                        list.Add(empregado);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get empregado list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static Empregado insertEmpregado(string nome, string nif, string email, string salario, int idCinema, string password)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertEmpregado", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NomeEmpregado", nome);
                cmd.Parameters.AddWithValue("@Nif", nif);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Salario", salario.Replace(',', '.'));
                if (idCinema != -1)
                    cmd.Parameters.AddWithValue("@idCinema", idCinema);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new Empregado(id, nome, nif, email, salario.Replace(',', '.'), idCinema);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert cinema failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static bool updateEmpregado(Empregado e)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateEmpregado", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdEmpregado", e.id);
                cmd.Parameters.AddWithValue("@NomeEmpregado", e.Title);
                cmd.Parameters.AddWithValue("@Nif", e.nif);
                cmd.Parameters.AddWithValue("@Email", e.email);
                cmd.Parameters.AddWithValue("@Salario", e.salario.Replace(',', '.'));
                if (e.cinema != -1)
                    cmd.Parameters.AddWithValue("@IdCinema", e.cinema);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.Print("Update empregado failed: " + ex.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeEmpregado(Empregado e)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeEmpregado", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdEmpregado", e.id);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.Print("Remove cinema failed: " + ex.Message);
                    cn.Close();
                    return false;
                }
            }

            public static List<Distribuidora> getDistribuidoraList()
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListDistribuidoras()", cn);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Distribuidora> list = new List<Distribuidora>();

                    while (reader.Read())
                    {
                        Distribuidora dist = new Distribuidora();
                        dist.id = int.Parse(reader["id_dist"].ToString());
                        dist.Title = reader["nome_dist"].ToString();
                        dist.precoInicial = reader["preco_inicial"].ToString();
                        dist.comissaoBilhete = reader["comissao_bilhete"].ToString();

                        list.Add(dist);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get distribuidora list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static Distribuidora insertDistribuidora(string nome, string pagamento, string comissao)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertDistribuidora", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NomeDist", nome);
                cmd.Parameters.AddWithValue("@PrecoInicial", pagamento.Replace(',', '.'));
                cmd.Parameters.AddWithValue("@ComissaoBilhete", comissao.Replace(',', '.'));
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new Distribuidora(id, nome, pagamento, comissao);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert distribuidora failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static bool updateDistribuidora(Distribuidora d)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateDistribuidora", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdDist", d.id);
                cmd.Parameters.AddWithValue("@NomeDist", d.Title);
                cmd.Parameters.AddWithValue("@PrecoInicial", d.precoInicial.Replace(',', '.'));
                cmd.Parameters.AddWithValue("@ComissaoBilhete", d.comissaoBilhete.Replace(',', '.'));

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Update distribuidora failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeDistribuidora(Distribuidora d)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeDistribuidora", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdDist", d.id);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Remove distribuidora failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static List<TipoBilhete> getTiposBilheteList()
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListTiposBilhete()", cn);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TipoBilhete> list = new List<TipoBilhete>();

                    while (reader.Read())
                    {
                        TipoBilhete tb = new TipoBilhete();
                        tb.id = int.Parse(reader["id_tipo_bilhete"].ToString());
                        tb.Title = reader["nome_bilhete"].ToString();
                        tb.restricoes= reader["restricoes"].ToString();
                        tb.custo = reader["custo"].ToString();

                        list.Add(tb);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get TipoBilhete list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static TipoBilhete insertTipoBilhete(string nome, string restricoes, string custo)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertTipoBilhete", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NomeBilhete", nome);
                cmd.Parameters.AddWithValue("@Restricoes", restricoes);
                cmd.Parameters.AddWithValue("@Custo", custo.Replace(',', '.'));
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new TipoBilhete(id, nome, restricoes, custo);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert TipoBilhete failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static bool updateTipoBilhete(TipoBilhete tb)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateTipoBilhete", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdTipoBilhete", tb.id);
                cmd.Parameters.AddWithValue("@NomeBilhete", tb.Title);
                cmd.Parameters.AddWithValue("@Restricoes", tb.restricoes);
                cmd.Parameters.AddWithValue("@Custo", tb.custo.Replace(',', '.'));

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Update TipoBilhete failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeTipoBilhete(TipoBilhete tb)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeTipoBilhete", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdTipoBilhete", tb.id);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Remove TipoBilhete failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static List<Sessao> getSessoesList(int idCinema, int diaSemana)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListSessoes(@IdCinema, @DiaSemana)", cn);
                cmd.Parameters.AddWithValue("@IdCinema", idCinema);
                cmd.Parameters.AddWithValue("@DiaSemana", diaSemana);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Sessao> list = new List<Sessao>();

                    while (reader.Read())
                    {
                        Sessao s = new Sessao();
                        s.id_cinema = int.Parse(reader["id_cinema"].ToString());
                        s.dia_semana = int.Parse(reader["dia_semana"].ToString());
                        string hora = reader["hora"].ToString();
                        s.hora = hora.Substring(0, hora.Length - 3);
                        s.desconto = reader["desconto"].ToString();

                        list.Add(s);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get sessao list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static Sessao insertSessao(int idCinema, int diaSemana, string hora, string desconto)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertSessao", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", idCinema);
                cmd.Parameters.AddWithValue("@DiaSemana", diaSemana);
                cmd.Parameters.AddWithValue("@Hora", hora);
                cmd.Parameters.AddWithValue("@Desconto", desconto);
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new Sessao(idCinema, diaSemana, hora, desconto);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert sessão failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static bool updateSessao(Sessao s, string novaHora)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateSessao", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", s.id_cinema);
                cmd.Parameters.AddWithValue("@DiaSemana", s.dia_semana);
                cmd.Parameters.AddWithValue("@Hora", s.hora);
                cmd.Parameters.AddWithValue("@Desconto", s.desconto);
                if(novaHora != null && s.hora != novaHora)
                    cmd.Parameters.AddWithValue("@NewHora", novaHora);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Update sessao failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeSessao(Sessao s)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeSessao", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", s.id_cinema);
                cmd.Parameters.AddWithValue("@DiaSemana", s.dia_semana);
                cmd.Parameters.AddWithValue("@Hora", s.hora);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Remove sessao failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static List<Sala> getSalasList(int idCinema)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListSalas(@IdCinema)", cn);
                cmd.Parameters.AddWithValue("@IdCinema", idCinema);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Sala> list = new List<Sala>();

                    while (reader.Read())
                    {
                        Sala s = new Sala();
                        s.id_cinema = int.Parse(reader["id_cinema"].ToString());
                        s.num_sala = int.Parse(reader["num_sala"].ToString());
                        s.num_filas = int.Parse(reader["num_filas"].ToString());
                        s.num_lugares_fila = int.Parse(reader["num_lugares_fila"].ToString());

                        list.Add(s);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get sessao list failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static Sala insertSala(int idCinema, string numSala, string numFilas, string numLugFila, DataTable dt)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertSala", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", idCinema);
                cmd.Parameters.AddWithValue("@NumSala", numSala);
                cmd.Parameters.AddWithValue("@NumFilas", numFilas);
                cmd.Parameters.AddWithValue("@NumLugaresFila", numLugFila);
                cmd.Parameters.AddWithValue("@Lugares", dt);
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new Sala(idCinema, int.Parse(numSala), int.Parse(numFilas), int.Parse(numLugFila), dt);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert sala failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }
            
            public static bool updateSala(Sala s, string newNumSala)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.updateSala", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", s.id_cinema);
                cmd.Parameters.AddWithValue("@NumSala", s.num_sala);
                if (newNumSala != null && s.num_sala.ToString() != newNumSala)
                    cmd.Parameters.AddWithValue("@NewNumSala", newNumSala);
                cmd.Parameters.AddWithValue("@Lugares", s.lugares);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Update sala failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static bool removeSala(Sala s)
            {
                if (!verifySGBDConnection())
                    return false;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.removeSala", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", s.id_cinema);
                cmd.Parameters.AddWithValue("@NumSala", s.num_sala);

                try
                {
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Print("Remove sala failed: " + e.Message);
                    cn.Close();
                    return false;
                }
            }

            public static InstanciaSessao insertInstanciaSessao(Sessao sessao, string dia, int numSala, int idFilme)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("CinemasZOZ.insertInstSessao", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCinema", sessao.id_cinema);
                cmd.Parameters.AddWithValue("@DiaSemana", sessao.dia_semana);
                cmd.Parameters.AddWithValue("@Hora", sessao.hora);
                cmd.Parameters.AddWithValue("@Dia", dia);
                cmd.Parameters.AddWithValue("@NumSala", numSala);
                cmd.Parameters.AddWithValue("@IdFilme", idFilme);
                cmd.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int, 4).Direction = System.Data.ParameterDirection.ReturnValue;

                try
                {
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@ReturnValue"].Value;
                    cn.Close();
                    return new InstanciaSessao(sessao, dia, numSala, idFilme);
                }
                catch (Exception e)
                {
                    Debug.Print("Insert InstanciaSala failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static List<Filme> getFilmesPorDiaList(int idCinema, string dia)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListFilmesPorDia(@IdCinema, @Dia)", cn);
                cmd.Parameters.AddWithValue("@IdCinema", idCinema);
                cmd.Parameters.AddWithValue("@Dia", dia);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Filme> list = new List<Filme>();

                    while (reader.Read())
                    {
                        Filme f = new Filme();
                        f.id = int.Parse(reader["id_filme"].ToString());
                        f.dist = int.Parse(reader["id_dist"].ToString());
                        f.Title = reader["titulo"].ToString();
                        f.idadeMin = int.Parse(reader["idade_min"].ToString());
                        f.duracao = int.Parse(reader["duracao"].ToString());
                        f.estreia = reader["estreia"].ToString();
                        f.idioma = reader["idioma"].ToString();

                        list.Add(f);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get getFilmesPorDiaList failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static List<InstanciaSessao> getListInstSessoesPorDiaFilme(int idCinema, string dia, int idFilme)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListInstSessoesPorDiaFilme(@IdCinema, @Dia, @IdFilme)", cn);
                cmd.Parameters.AddWithValue("@IdCinema", idCinema);
                cmd.Parameters.AddWithValue("@Dia", dia);
                cmd.Parameters.AddWithValue("@IdFilme", idFilme);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<InstanciaSessao> list = new List<InstanciaSessao>();
                    
                    while (reader.Read())
                    {
                        InstanciaSessao s = new InstanciaSessao();
                        s.sessao = new Sessao();
                        s.sessao.id_cinema = int.Parse(reader["id_cinema"].ToString());
                        s.sessao.dia_semana = int.Parse(reader["dia_semana"].ToString());
                        s.sessao.hora = reader["hora"].ToString();
                        s.dia = reader["dia"].ToString();
                        s.num_sala = int.Parse(reader["num_sala"].ToString());
                        s.id_filme = int.Parse(reader["id_filme"].ToString());

                        list.Add(s);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get ListInstSessoesPorDiaFilme failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }

            public static List<Lugar> getListLugaresInstSessao(int idCinema, int diaSemana, string hora, string dia, int numSala)
            {
                if (!verifySGBDConnection())
                    return null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.getListLugaresInstSessao (@IdCinema, @DiaSemana, @Hora, @Dia, @NumSala)", cn);
                cmd.Parameters.AddWithValue("@IdCinema", idCinema);
                cmd.Parameters.AddWithValue("@DiaSemana", diaSemana);
                cmd.Parameters.AddWithValue("@Hora", hora);
                cmd.Parameters.AddWithValue("@Dia", dia);
                cmd.Parameters.AddWithValue("@NumSala", numSala);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Lugar> list = new List<Lugar>();

                    while (reader.Read())
                    {
                        Lugar l = new Lugar();
                        l.id_cinema = int.Parse(reader["id_cinema"].ToString());
                        l.num_sala = int.Parse(reader["num_sala"].ToString());
                        l.fila = int.Parse(reader["fila"].ToString());
                        l.num_lugar = int.Parse(reader["num_lugar"].ToString());
                        l.ocupado = bool.Parse(reader["ocupado"].ToString());

                        list.Add(l);
                    }
                    cn.Close();
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Print("Get getListLugaresInstSessao failed: " + e.Message);
                    cn.Close();
                    return null;
                }
            }
        }

        public static class ManagerCommands
        {
            
        }

        public static class CommonCommands
        {

        }

        
        public static List<Empregado> getEmpregadoList()
        {
            if (!verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CinemasZOZ.Empregado ORDER BY nome_empregado", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Empregado> list = new List<Empregado>();
            
            while(reader.Read())
            {
                int id = int.Parse(reader["id_empregado"].ToString());
                string nome = reader["nome_empregado"].ToString();
                string nif = reader["nif"].ToString();
                string email = reader["email"].ToString();
                string telemovel = reader["telemovel"].ToString();
                string salario = reader["salario"].ToString();
                int idCinema = int.Parse(reader["cinema"].ToString());

                list.Add(new Empregado(id, nome, nif, email, salario, idCinema));
            }
            cn.Close();
            return list;
        }

        public static bool updateEmpregado(Empregado e)
        {
            if (!verifySGBDConnection())
                return false;
            SqlCommand cmd = new SqlCommand("UPDATE CinemasZOZ.Empregado SET nome_empregado = @nome, nif = @nif, email = @email, telemovel = @telemovel, salario = @salario, cinema = @IdCinema WHERE id_empregado = @idEmpregado", cn);
            cmd.Parameters.AddWithValue("@nome", e.Title);
            cmd.Parameters.AddWithValue("@nif", e.nif);
            cmd.Parameters.AddWithValue("@email", e.email);
            cmd.Parameters.AddWithValue("@salario", e.salario);
            cmd.Parameters.AddWithValue("@idCinema", e.cinema);
            cmd.Parameters.AddWithValue("@idEmpregado", e.id);

            cmd.ExecuteNonQuery();
            cn.Close();
            return true;
        }

        public static Empregado insertEmpregado(string nome, string nif, string email, string salario, bool isAdmin, int idCinema)
        {
            if (!verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("INSERT CinemasZOZ.Empregado (nome_empregado, nif, email, telemovel, salario, cinema) Output Inserted.id_empregado VALUES(@nome, @nif, @email, @telemovel, @salario, @IdCinema)", cn);
            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@nif", nif);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@salario", salario);
            cmd.Parameters.AddWithValue("@idCinema", idCinema);

            int id = (int) cmd.ExecuteScalar();
            cn.Close();

            return new Empregado(id, nome, nif, email, salario, idCinema);
        }
    }
}