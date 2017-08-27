using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;
using System.Data.SqlClient;
using System.Data;

namespace b_KnowLedge.ViewModels
{
    class Sincroniza
    {
        SqlConnectionStringBuilder connString2Builder = new SqlConnectionStringBuilder();
        Classes.DataControl dataControl = new Classes.DataControl();

        public bool TesteConnectionServer(string servidor, string baseDeDados, string userName, string password)
        {
            bool done = true;
            connString2Builder.DataSource = servidor;
            connString2Builder.InitialCatalog = baseDeDados;
            connString2Builder.UserID = userName;
            connString2Builder.Password = password;

            using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                    }
                    catch
                    {
                        done = false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return done;
        }

        public int ImportEntidades()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {

                var dataCompara = (from u in bd.Entidades
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM Entidades WHERE USRDATA > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM Entidades";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPENTIDADE");
                                var nr = (from u in bd.Entidades
                                          where u.StampEntidade == id
                                          select u.StampEntidade).FirstOrDefault();

                                Entidades entidade = null;


                                if (nr == null || nr == "")
                                {
                                    entidade = new Entidades();
                                }
                                else
                                {
                                    entidade = bd.Entidades.Single(u => u.StampEntidade == nr);
                                }

                                entidade.StampEntidade = dr1["STAMPENTIDADE"].ToString();
                                entidade.Nome = dr1["NOME"].ToString();
                                entidade.Morada = dr1["MORADA"].ToString();
                                entidade.Localidade = dr1["LOCALIDADE"].ToString();
                                entidade.CodPostal = dr1["CODPOSTAL"].ToString();
                                entidade.Telemovel = dr1["TELEMOVEL"].ToString();
                                entidade.Telefone = dr1["TELEFONE"].ToString();
                                entidade.Fax = dr1["FAX"].ToString();
                                entidade.Numero = dr1["NUMERO"].ToString();
                                entidade.Email = dr1["EMAIL"].ToString();
                                entidade.Site = dr1["SITE"].ToString();

                                try
                                {
                                    entidade.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    entidade.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                entidade.Ousrhora = dr1["OUSRHORA"].ToString();
                                try
                                {
                                    entidade.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    entidade.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                entidade.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                entidade.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Entidades.Add(entidade);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;


                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[Entidades] SET [DataSincronizacao] = @data WHERE STAMPENTIDADE = @IDENTIDADE";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDENTIDADE", entidade.StampEntidade);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }

                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ImportProjetos()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {

                var dataCompara = (from u in bd.Projetos
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM Projetos WHERE usrdata > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM Projetos";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPPROJETO");
                                var nr = (from u in bd.Projetos
                                          where u.StampProjeto == id
                                          select u.StampProjeto).FirstOrDefault();

                                Projetos projeto = null;


                                if (nr == null || nr == "")
                                {
                                    projeto = new Projetos();
                                }
                                else
                                {
                                    projeto = bd.Projetos.Single(u => u.StampProjeto == nr);
                                }

                                projeto.StampProjeto = dr1["STAMPPROJETO"].ToString();
                                projeto.StampEntidade = dr1["STAMPENTIDADE"].ToString();
                                projeto.Nome = dr1["NOME"].ToString();
                                projeto.Morada = dr1["MORADA"].ToString();
                                projeto.Localidade = dr1["LOCALIDADE"].ToString();
                                projeto.CodPostal = dr1["CODPOSTAL"].ToString();
                                projeto.Telemovel = dr1["TELEMOVEL"].ToString();
                                projeto.Telefone = dr1["TELEFONE"].ToString();
                                projeto.Fax = dr1["FAX"].ToString();
                                projeto.Email = dr1["EMAIL"].ToString();
                                projeto.Site = dr1["SITE"].ToString();
                                projeto.Descricacao = dr1["DESCRICACAO"].ToString();

                                try
                                {
                                    projeto.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    projeto.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                projeto.Ousrhora = dr1["OUSRHORA"].ToString();
                                try
                                {
                                    projeto.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    projeto.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                projeto.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                projeto.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Projetos.Add(projeto);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[Projetos] SET [DataSincronizacao] = @data WHERE STAMPPROJETO = @IDPROJETO";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDPROJETO", projeto.StampProjeto);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ImportUtilizadores()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {

                var dataCompara = (from u in bd.Utilizadores
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM UTILIZADORES WHERE usrdata > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM UTILIZADORES";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPUTILIZADOR");
                                var nr = (from u in bd.Utilizadores
                                          where u.StampUtilizador == id
                                          select u.StampUtilizador).FirstOrDefault();

                                Utilizadores utl = null;

                                if (nr == null || nr == "")
                                {
                                    utl = new Utilizadores();
                                }
                                else
                                {
                                    utl = bd.Utilizadores.Single(u => u.StampUtilizador == nr);
                                }

                                utl.StampUtilizador = dr1["STAMPUTILIZADOR"].ToString();
                                utl.Nome = dr1["NOME"].ToString();
                                utl.Morada = dr1["MORADA"].ToString();
                                utl.Localidade = dr1["LOCALIDADE"].ToString();
                                utl.CodPostal = dr1["CODPOSTAL"].ToString();
                                utl.Telemovel = dr1["TELEMOVEL"].ToString();
                                utl.Username = dr1["USERNAME"].ToString();
                                utl.Password = dr1["PASSWORD"].ToString();
                                utl.Email = dr1["EMAIL"].ToString();

                                try
                                {
                                    if (dr1["FOTO"].ToString().Trim() != "")
                                        utl.Foto = (byte[])dr1["FOTO"];
                                }
                                catch { }

                                try
                                {
                                    utl.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    utl.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                utl.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    utl.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    utl.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                utl.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                utl.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Utilizadores.Add(utl);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[Utilizadores] SET [DataSincronizacao] = @data WHERE STAMPUTILIZADOR= @IDUSER";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDUSER", utl.StampUtilizador);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }



        public int ImportPessoas()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {
                var dataCompara = (from u in bd.Pessoas
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM PESSOAS WHERE usrdata > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM PESSOAS";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPPESSOA");
                                var nr = (from u in bd.Pessoas
                                          where u.StampPessoa == id
                                          select u.StampPessoa).FirstOrDefault();

                                Pessoas pes = null;

                                if (nr == null || nr == "")
                                {
                                    pes = new Pessoas();
                                }
                                else
                                {
                                    pes = bd.Pessoas.Single(u => u.StampPessoa == nr);
                                }

                                pes.StampPessoa = dr1["STAMPPESSOA"].ToString();
                                pes.StampEntidades = dr1["STAMPENTIDADES"].ToString();
                                pes.Nome = dr1["NOME"].ToString();
                                pes.Morada = dr1["MORADA"].ToString();
                                pes.Localidade = dr1["LOCALIDADE"].ToString();
                                pes.CodPostal = dr1["CODPOSTAL"].ToString();
                                pes.Telemovel = dr1["TELEMOVEL"].ToString();
                                pes.Telefone = dr1["TELEFONE"].ToString();
                                pes.Fax = dr1["FAX"].ToString();
                                pes.Numero = dr1["NUMERO"].ToString();
                                pes.Email = dr1["EMAIL"].ToString();
                                pes.Site = dr1["SITE"].ToString();

                                try
                                {
                                    if (dr1["FOTO"].ToString().Trim() != "")
                                        pes.Foto = (byte[])dr1["FOTO"];
                                }
                                catch { }

                                try
                                {
                                    pes.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    pes.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                pes.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    pes.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    pes.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                pes.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                pes.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Pessoas.Add(pes);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[Pessoas] SET [DataSincronizacao] = @data WHERE STAMPPESSOA = @IDPESSOA";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDPESSOA", pes.StampPessoa);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }

        public int ImportBDs()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {
                var dataCompara = (from u in bd.BasesDados
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM BasesDados WHERE USRDATA > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM BasesDados";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPBASEDADOS");
                                var nr = (from u in bd.BasesDados
                                          where u.StampBaseDados == id
                                          select u.StampBaseDados).FirstOrDefault();

                                BasesDados database = null;

                                if (nr == null || nr == "")
                                {
                                    database = new BasesDados();
                                }
                                else
                                {
                                    database = bd.BasesDados.Single(u => u.StampBaseDados == nr);
                                }

                                database.StampBaseDados = dr1["STAMPBASEDADOS"].ToString();
                                database.StampProjeto = dr1["STAMPPROJETO"].ToString();
                                database.Servername = dr1["SERVERNAME"].ToString();
                                database.UserID = dr1["USERID"].ToString();
                                database.Password = dr1["PASSWORD"].ToString();
                                database.Initialcatalog = dr1["INITIALCATALOG"].ToString();

                                try
                                {
                                    if (dr1["ENCRYPT"].ToString().Trim() != "" && dr1["ENCRYPT"] != null)
                                        database.Encrypt = Convert.ToBoolean(dr1["ENCRYPT"].ToString());
                                }
                                catch { }

                                try
                                {
                                    if (dr1["TRUSTSERVERCERTIFICATE"].ToString().Trim() != "" && dr1["TRUSTSERVERCERTIFICATE"] != null)
                                        database.Trustservercertificate = Convert.ToBoolean(dr1["TRUSTSERVERCERTIFICATE"].ToString());
                                }
                                catch { }

                                try
                                {
                                    database.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    database.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                database.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    database.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    database.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                database.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                database.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.BasesDados.Add(database);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[BasesDados] SET [DataSincronizacao] = @data WHERE STAMPBASEDADOS = @IDBD";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDBD", database.StampBaseDados);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ImportTipo()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {
                var dataCompara = (from u in bd.Tipos
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM TIPOS WHERE USRDATA > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM TIPOS";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPTIPO");
                                var nr = (from u in bd.Tipos
                                          where u.StampTipo == id
                                          select u.StampTipo).FirstOrDefault();

                                Tipos type = null;

                                if (nr == null || nr == "")
                                {
                                    type = new Tipos();
                                }
                                else
                                {
                                    type = bd.Tipos.Single(u => u.StampTipo == nr);
                                }

                                type.StampTipo = dr1["STAMPTIPO"].ToString();
                                type.Nome = dr1["NOME"].ToString();
                                type.Descricao = dr1["DESCRICAO"].ToString();

                                try
                                {
                                    type.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    type.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                type.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    type.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    type.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                type.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                type.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Tipos.Add(type);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[TIPOS] SET [DataSincronizacao] = @data WHERE STAMPTIPO = @IDTIPO";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDTIPO", type.StampTipo);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ImportSubtipo()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {
                var dataCompara = (from u in bd.Subtipos
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM SUBTIPOS WHERE USRDATA > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM SUBTIPOS";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPSUBTIPO");
                                var nr = (from u in bd.Subtipos
                                          where u.StampSubtipo == id
                                          select u.StampSubtipo).FirstOrDefault();

                                Subtipos subtype = null;

                                if (nr == null || nr == "")
                                {
                                    subtype = new Subtipos();
                                }
                                else
                                {
                                    subtype = bd.Subtipos.Single(u => u.StampSubtipo == nr);
                                }

                                subtype.StampSubtipo = dr1["STAMPSUBTIPO"].ToString();

                                try
                                {
                                    subtype.StampTipo = dr1["STAMPTIPO"].ToString();
                                }
                                catch { }
                                subtype.Nome = dr1["NOME"].ToString();
                                subtype.Descricao = dr1["DESCRICAO"].ToString();

                                try
                                {
                                    subtype.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    subtype.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                subtype.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    subtype.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    subtype.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                subtype.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                subtype.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Subtipos.Add(subtype);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;
                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[SUBTIPOS] SET [DataSincronizacao] = @data WHERE STAMPSUBTIPO = @IDSUBTIPO";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDSUBTIPO", subtype.StampSubtipo);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ImportTabelas()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {
                var dataCompara = (from u in bd.Tabelas
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM TABELAS WHERE USRDATA > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM TABELAS";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPTABELA");
                                var nr = (from u in bd.Tabelas
                                          where u.StampTabela == id
                                          select u.StampTabela).FirstOrDefault();

                                Tabelas tab = null;


                                if (nr == null || nr == "")
                                {
                                    tab = new Tabelas();
                                }
                                else
                                {
                                    tab = bd.Tabelas.Single(u => u.StampTabela == nr);
                                }

                                tab.StampTabela = dr1["STAMPTABELA"].ToString();

                                try
                                {
                                    tab.StampTipo = dr1["STAMPTIPO"].ToString();
                                }
                                catch { }

                                try
                                {
                                    tab.StampSubtipo = dr1["STAMPSUBTIPO"].ToString();
                                }
                                catch { }

                                tab.Descricao = dr1["DESCRICAO"].ToString();
                                tab.QueryString = dr1["QUERYSTRING"].ToString();

                                try
                                {
                                    tab.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    tab.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                tab.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    tab.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    tab.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                tab.Usrhora = dr1["USRHORA"].ToString();
                                DateTime dateTime = dataControl.GeraDataHora();
                                tab.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Tabelas.Add(tab);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[Tabelas] SET [DataSincronizacao] = @data WHERE STAMPTABELA = @IDTABELA";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDTABELA", tab.StampTabela);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ImportConhecimento()
        {
            int conta = 0;
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();

            try
            {
                var dataCompara = (from u in bd.Conhecimentos
                                   select u.DataSincronizacao).Max();

                using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            if (dataCompara != null)
                            {
                                command.CommandText = "SELECT * FROM CONHECIMENTOS WHERE USRDATA > @DATAS";
                                command.Parameters.AddWithValue("DATAS", dataCompara);
                            }
                            else
                            {
                                command.CommandText = "SELECT * FROM CONHECIMENTOS";
                            }

                            conn.Open();
                            SqlDataReader dr = command.ExecuteReader();
                            dt = new DataTable();

                            dt.Load(dr);
                            conn.Close();

                            foreach (DataRow dr1 in dt.Rows)
                            {
                                string id = dr1.Field<string>("STAMPCONHECIMENTO");
                                var nr = (from u in bd.Conhecimentos
                                          where u.StampConhecimento == id
                                          select u.StampConhecimento).FirstOrDefault();

                                Conhecimentos conhecimento = null;


                                if (nr == null || nr == "")
                                {
                                    conhecimento = new Conhecimentos();
                                }
                                else
                                {
                                    conhecimento = bd.Conhecimentos.Single(u => u.StampConhecimento == nr);
                                }

                                conhecimento.StampConhecimento = dr1["STAMPCONHECIMENTO"].ToString();
                                try
                                {
                                    conhecimento.StampProjeto = dr1["STAMPPROJETO"].ToString();
                                }
                                catch { }

                                try
                                {
                                    conhecimento.StampTipo = dr1["STAMPTIPO"].ToString();
                                }
                                catch { }

                                try
                                {
                                    conhecimento.StampSubtipo = dr1["STAMPSUBTIPO"].ToString();
                                }
                                catch { }

                                conhecimento.Descricao = dr1["DESCRICAO"].ToString();
                                conhecimento.Codigo = dr1["CODIGO"].ToString();
                                conhecimento.Metadados = dr1["METADADOS"].ToString();
                                conhecimento.Ecra = dr1["ECRA"].ToString();
                                conhecimento.Mensagem = dr1["MENSAGEM"].ToString();
                                conhecimento.Teclas = dr1["TECLAS"].ToString();
                                conhecimento.Tabela = dr1["TABELA"].ToString();

                                try
                                {
                                    conhecimento.NivelAprovacao = Convert.ToInt32(dr1["NIVELAPROVACAO"]);
                                }
                                catch { }

                                try
                                {
                                    conhecimento.Ousrinis = dr1["OUSRINIS"].ToString();
                                }
                                catch { }

                                try
                                {
                                    conhecimento.Ousrdata = Convert.ToDateTime(dr1["OUSRDATA"]);
                                }
                                catch { }

                                conhecimento.Ousrhora = dr1["OUSRHORA"].ToString();

                                try
                                {
                                    conhecimento.Usrinis = dr1["USRINIS"].ToString();
                                }
                                catch { }
                                try
                                {
                                    conhecimento.Usrdata = Convert.ToDateTime(dr1["USRDATA"]);
                                }
                                catch { }
                                conhecimento.Usrhora = dr1["USRHORA"].ToString();

                                DateTime dateTime = dataControl.GeraDataHora();
                                conhecimento.DataSincronizacao = dateTime;

                                if (nr == null || nr == "")
                                    bd.Conhecimentos.Add(conhecimento);

                                try
                                {
                                    bd.SaveChanges();
                                    conta++;

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {
                                                command2.CommandText = "UPDATE [dbo].[Conhecimentos] SET [DataSincronizacao] = @data WHERE STAMPCONHECIMENTO = @IDCON";
                                                command2.Parameters.Add("data", dateTime);
                                                command2.Parameters.Add("IDCON", conhecimento.StampConhecimento);

                                                conn2.Open();
                                                command2.ExecuteNonQuery();
                                                conn2.Close();
                                                command2.Parameters.Clear();
                                            }
                                            catch { }
                                        }
                                    }

                                    using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                                    {
                                        using (SqlCommand command2 = conn2.CreateCommand())
                                        {
                                            try
                                            {

                                                command2.CommandText = "SELECT * FROM ANEXOS WHERE STAMPCONHECIMENTO = @ID";
                                                command2.Parameters.AddWithValue("ID", id);


                                                DataTable dt2;
                                                conn2.Open();
                                                SqlDataReader dr4 = command2.ExecuteReader();
                                                dt2 = new DataTable();

                                                dt2.Load(dr4);
                                                conn2.Close();

                                                foreach (DataRow dr3 in dt2.Rows)
                                                {

                                                    Anexos anexo = new Anexos();
                                                    anexo.StampAnexo = dr3["STAMPANEXO"].ToString();
                                                    try
                                                    {
                                                        anexo.StampConhecimento = dr3["STAMPCONHECIMENTO"].ToString();
                                                    }
                                                    catch { }

                                                    anexo.NomeFicheiro = dr3["NomeFicheiro"].ToString();
                                                    try
                                                    {
                                                        if (dr3["FICHEIRO"].ToString().Trim() != "")
                                                            anexo.Ficheiro = (byte[])dr3["FICHEIRO"];
                                                    }
                                                    catch { }

                                                    try
                                                    {
                                                        anexo.Ousrinis = dr3["OUSRINIS"].ToString();
                                                    }
                                                    catch { }

                                                    try
                                                    {
                                                        anexo.Ousrdata = Convert.ToDateTime(dr3["OUSRDATA"]);
                                                    }
                                                    catch { }

                                                    anexo.Ousrhora = dr3["OUSRHORA"].ToString();

                                                    try
                                                    {
                                                        anexo.Usrinis = dr3["USRINIS"].ToString();
                                                    }
                                                    catch { }
                                                    try
                                                    {
                                                        anexo.Usrdata = Convert.ToDateTime(dr3["USRDATA"]);
                                                    }
                                                    catch { }
                                                    anexo.Usrhora = dr3["USRHORA"].ToString();


                                                    bd.Anexos.Add(anexo);

                                                    try
                                                    {
                                                        bd.SaveChanges();
                                                    }
                                                    catch { }

                                                }
                                            }
                                            catch { }
                                        }
                                    }



                                }
                                catch { }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch
            {

            }

            return conta;
        }


        public int ExportConhecimento()
        {
            int conta = 0;
            string idConhecimentos = "";
            DataTable dt;
            BDKnowLedge bd = new BDKnowLedge();
            List<Conhecimentos> conhecimento;
            try
            {
                var table = (from u in bd.Conhecimentos
                             where u.Usrdata > u.DataSincronizacao || u.DataSincronizacao == null
                             select u);
                conhecimento = table.ToList<Conhecimentos>();

                foreach (Conhecimentos c in conhecimento)
                {
                    int nr = 0;
                    using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
                    {
                        using (SqlCommand command = conn.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "SELECT STAMPPROJETO FROM PROJETOS WHERE STAMPPROJETO = @idPRO";
                                command.Parameters.AddWithValue("idPRO", c.StampProjeto);

                                conn.Open();
                                var num1 = command.ExecuteScalar();
                                conn.Close();
                                command.Parameters.Clear();
                                int num = 0;
                                try
                                {
                                    if (num1 != null && num1.ToString().Trim() != "")
                                        num++;
                                }
                                catch { }
                                if (num > 0)
                                    nr++;

                                command.CommandText = "SELECT STAMPTIPO FROM TIPOS WHERE STAMPTIPO = @idTIP";
                                command.Parameters.AddWithValue("idTIP", c.StampTipo);

                                conn.Open();
                                num1 = command.ExecuteScalar();
                                conn.Close();
                                command.Parameters.Clear();
                                try
                                {
                                    if (num1 != null && num1.ToString().Trim() != "")
                                        num++;
                                }
                                catch { }
                                if (num > 0)
                                    nr++;

                                command.CommandText = "SELECT STAMPSUBTIPO FROM SUBTIPOS WHERE STAMPSUBTIPO = @iSUBTIP";
                                command.Parameters.AddWithValue("iSUBTIP", c.StampSubtipo);

                                conn.Open();
                                num1 = command.ExecuteScalar();
                                conn.Close();
                                command.Parameters.Clear();
                                try
                                {
                                    if (num1 != null && num1.ToString().Trim() != "")
                                        num++;
                                }
                                catch { }
                                if (num > 0)
                                    nr++;


                                command.CommandText = "SELECT STAMPCONHECIMENTO FROM CONHECIMENTOS WHERE STAMPCONHECIMENTO = @idCON";
                                command.Parameters.AddWithValue("idCON", c.StampConhecimento);

                                conn.Open();
                                var IDcon = command.ExecuteScalar();
                                conn.Close();
                                command.Parameters.Clear();

                                try
                                {
                                    idConhecimentos = IDcon.ToString();
                                }
                                catch { }

                            }
                            catch { }
                        }
                    }

                    if (nr == 3 && (idConhecimentos == ""))
                    {
                        using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                        {
                            using (SqlCommand command2 = conn2.CreateCommand())
                            {
                                try
                                {
                                    command2.CommandText = "INSERT INTO [dbo].[Conhecimentos] ([STAMPCONHECIMENTO],[STAMPProjeto],[STAMPSubtipo],[STAMPTipo],[Descricao],[Codigo],[Metadados],[Ecra],[Mensagem],[Teclas],[Tabela],[NivelAprovacao],[Ousrinis],[Ousrdata],[Ousrhora],[Usrinis],[Usrdata],[Usrhora],[DataSincronizacao]) VALUES (@IDCON, @IDPROJETO, @IDSUBTIPO, @IDTIPO, @DESC, @COD, @META, @ECRAS, @MSG, @TEC, @TAB, @NIVEL, @OINIS, @ODATA, @OHORA, @UINIS, @UDATA, @UHORA, @DATAS)";
                                    command2.Parameters.Add("IDCON", c.StampConhecimento);
                                    command2.Parameters.Add("IDPROJETO", c.StampProjeto);
                                    command2.Parameters.Add("IDSUBTIPO", c.StampSubtipo);
                                    command2.Parameters.Add("IDTIPO", c.StampTipo);
                                    command2.Parameters.Add("DESC", c.Descricao);
                                    command2.Parameters.Add("COD", c.Codigo);
                                    command2.Parameters.Add("META", c.Metadados);
                                    command2.Parameters.Add("ECRAS", c.Ecra);
                                    command2.Parameters.Add("MSG", c.Mensagem);
                                    command2.Parameters.Add("TEC", c.Teclas);
                                    command2.Parameters.Add("TAB", c.Tabela);
                                    command2.Parameters.Add("NIVEL", c.NivelAprovacao);
                                    command2.Parameters.Add("OINIS", c.Ousrinis);
                                    command2.Parameters.Add("ODATA", c.Ousrdata);
                                    command2.Parameters.Add("OHORA", c.Ousrhora);
                                    command2.Parameters.Add("UINIS", c.Usrinis);
                                    command2.Parameters.Add("UDATA", c.Usrdata);
                                    command2.Parameters.Add("UHORA", c.Usrhora);
                                    DateTime dt1 = DateTime.Now;
                                    command2.Parameters.Add("DATAS", dt1);

                                    int nr2 = 0;
                                    conn2.Open();
                                    try
                                    {
                                        nr2 = command2.ExecuteNonQuery();
                                        command2.Parameters.Clear();
                                    }
                                    catch
                                    {

                                    }
                                    finally
                                    {
                                        command2.Parameters.Clear();
                                    }
                                    conn2.Close();

                                    if (nr2 > 0)
                                    {
                                        c.DataSincronizacao = dt1;

                                        try
                                        {
                                            bd.SaveChanges();
                                            conta++;
                                            var tableAnexo = (from u in bd.Anexos
                                                              where u.StampConhecimento == c.StampConhecimento
                                                              select u);

                                            if (tableAnexo != null)
                                            {
                                                List<Anexos> anexos = tableAnexo.ToList<Anexos>();
                                                foreach (Anexos anexo in anexos)
                                                {
                                                    command2.CommandText = "INSERT INTO [dbo].[Anexos]([StampAnexo],[STAMPConhecimento],[NomeFicheiro],[Ficheiro],[Ousrinis],[Ousrdata],[Ousrhora],[Usrinis],[Usrdata],[Usrhora]) VALUES (@IDANE, @IDCON, @NOMEFILE, @FILE, @OUSER, @ODATA, @OHORA, @UISER, @UDATA, @UHORA)";
                                                    command2.Parameters.Add("IDANE", anexo.StampAnexo);
                                                    command2.Parameters.Add("IDCON", anexo.StampConhecimento);
                                                    command2.Parameters.Add("NOMEFILE", anexo.NomeFicheiro);
                                                    command2.Parameters.Add("FILE", anexo.Ficheiro);
                                                    command2.Parameters.Add("OUSER", anexo.Ousrinis);
                                                    command2.Parameters.Add("ODATA", anexo.Ousrdata);
                                                    command2.Parameters.Add("OHORA", anexo.Ousrhora);
                                                    command2.Parameters.Add("UISER", anexo.Usrinis);
                                                    command2.Parameters.Add("UDATA", anexo.Usrdata);
                                                    command2.Parameters.Add("UHORA", anexo.Usrhora);


                                                    conn2.Open();
                                                    try
                                                    {
                                                        int nr3 = command2.ExecuteNonQuery();
                                                        command2.Parameters.Clear();
                                                    }
                                                    catch
                                                    {

                                                    }
                                                    finally
                                                    {
                                                        command2.Parameters.Clear();
                                                    }
                                                    conn2.Close();

                                                }
                                            }

                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }

                    }
                    else
                    {
                        //************************************************************************************************************************

                        using (SqlConnection conn2 = new SqlConnection(connString2Builder.ToString()))
                        {
                            using (SqlCommand command2 = conn2.CreateCommand())
                            {
                                try
                                {
                                    command2.CommandText = "UPDATE [dbo].[Conhecimentos] SET [STAMPCONHECIMENTO] = @IDCON, [STAMPProjeto] = @IDPROJETO, [STAMPSubtipo] = @IDSUBTIPO ,[STAMPTipo] = @IDTIPO ,[Descricao] = @DESC ,[Codigo] = @COD ,[Metadados] = @META ,[Ecra] = @ECRAS ,[Mensagem] = @MSG ,[Teclas] = @TEC,[Tabela] = @TAB,[NivelAprovacao] = @NIVEL,[Ousrinis] = @OINIS,[Ousrdata] = @ODATA,[Ousrhora] = @OHORA,[Usrinis] = @UINIS,[Usrdata] = @UDATA,[Usrhora] = @UHORA,[DataSincronizacao] = @DATAS WHERE STAMPCONHECIMENTO = @CONHECIMENTOID";
                                    command2.Parameters.Add("IDCON", c.StampConhecimento);
                                    command2.Parameters.Add("IDPROJETO", c.StampProjeto);
                                    command2.Parameters.Add("IDSUBTIPO", c.StampSubtipo);
                                    command2.Parameters.Add("IDTIPO", c.StampTipo);
                                    command2.Parameters.Add("DESC", c.Descricao);
                                    command2.Parameters.Add("COD", c.Codigo);
                                    command2.Parameters.Add("META", c.Metadados);
                                    command2.Parameters.Add("ECRAS", c.Ecra);
                                    command2.Parameters.Add("MSG", c.Mensagem);
                                    command2.Parameters.Add("TEC", c.Teclas);
                                    command2.Parameters.Add("TAB", c.Tabela);
                                    command2.Parameters.Add("NIVEL", c.NivelAprovacao);
                                    command2.Parameters.Add("OINIS", c.Ousrinis);
                                    command2.Parameters.Add("ODATA", c.Ousrdata);
                                    command2.Parameters.Add("OHORA", c.Ousrhora);
                                    command2.Parameters.Add("UINIS", c.Usrinis);
                                    command2.Parameters.Add("UDATA", c.Usrdata);
                                    command2.Parameters.Add("UHORA", c.Usrhora);
                                    DateTime dt1 = DateTime.Now;
                                    command2.Parameters.Add("DATAS", dt1);
                                    command2.Parameters.Add("CONHECIMENTOID", idConhecimentos);

                                    int nr2 = 0;
                                    conn2.Open();
                                    try
                                    {
                                        nr2 = command2.ExecuteNonQuery();
                                        command2.Parameters.Clear();
                                    }
                                    catch
                                    {

                                    }
                                    finally
                                    {
                                        command2.Parameters.Clear();
                                    }
                                    conn2.Close();

                                    if (nr2 > 0)
                                    {
                                        c.DataSincronizacao = dt1;

                                        try
                                        {
                                            bd.SaveChanges();
                                            conta++;
                                            var tableAnexo = (from u in bd.Anexos
                                                              where u.StampConhecimento == c.StampConhecimento
                                                              select u);

                                            if (tableAnexo != null)
                                            {

                                                List<Anexos> anexos = tableAnexo.ToList<Anexos>();
                                                foreach (Anexos anexo in anexos)
                                                {

                                                    command2.CommandText = "SELECT STAMPANEXO FROM ANEXOS WHERE STAMPCONHECIMENTO = @idCON";
                                                    command2.Parameters.AddWithValue("idCON", c.StampConhecimento);

                                                    conn2.Open();
                                                    var IDan = command2.ExecuteScalar();
                                                    conn2.Close();
                                                    command2.Parameters.Clear();
                                                    string anexosID = "";
                                                    try
                                                    {
                                                        anexosID = IDan.ToString();
                                                    }
                                                    catch { }

                                                    if (anexosID != "")
                                                    {
                                                        command2.CommandText = "INSERT INTO [dbo].[Anexos]([STAMPANEXO],[STAMPConhecimento],[NomeFicheiro],[Ficheiro],[Ousrinis],[Ousrdata],[Ousrhora],[Usrinis],[Usrdata],[Usrhora]) VALUES (@IDANE, @IDCON, @NOMEFILE, @FILE, @OUSER, @ODATA, @OHORA, @UISER, @UDATA, @UHORA)";
                                                        command2.Parameters.Add("IDANE", anexo.StampAnexo);
                                                        command2.Parameters.Add("IDCON", anexo.StampConhecimento);
                                                        command2.Parameters.Add("NOMEFILE", anexo.NomeFicheiro);
                                                        command2.Parameters.Add("FILE", anexo.Ficheiro);
                                                        command2.Parameters.Add("OUSER", anexo.Ousrinis);
                                                        command2.Parameters.Add("ODATA", anexo.Ousrdata);
                                                        command2.Parameters.Add("OHORA", anexo.Ousrhora);
                                                        command2.Parameters.Add("UISER", anexo.Usrinis);
                                                        command2.Parameters.Add("UDATA", anexo.Usrdata);
                                                        command2.Parameters.Add("UHORA", anexo.Usrhora);


                                                        conn2.Open();
                                                        try
                                                        {
                                                            int nr3 = command2.ExecuteNonQuery();
                                                            command2.Parameters.Clear();
                                                        }
                                                        catch
                                                        {
                                                        }
                                                        finally
                                                        {
                                                            command2.Parameters.Clear();
                                                        }
                                                        conn2.Close();

                                                    }

                                                }
                                            }

                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }


                        }


                        //***********************************************************************************************************************
                    }

                }

            }
            catch { }


            return conta;

        }


    }
}
