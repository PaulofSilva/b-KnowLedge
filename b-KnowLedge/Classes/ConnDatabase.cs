using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Metadata.Edm;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Windows;
using System.Windows.Controls;
using b_KnowLedge.Properties;
using b_KnowLedge;
using Microsoft.SqlServer.Management.Common;
using System.Text.RegularExpressions;





namespace b_KnowLedge.Classes
{
    /// <summary>
    /// Estabelece a conexão com a Base de Dados.
    /// </summary><remarks>
    /// Preenche a connectionString do App.Config, faz um teste à conexão e preenche os settings da Aplicação. 
    /// </remarks>
    class ConnDatabase
    {
        /// <summary>
        /// A connectionString da App.Config é preenchida.
        /// </summary>
        /// <remarks>
        /// A connectionString é alterada em Runtime com os dados presentes no settings da aplicação
        /// </remarks>
        public bool ConnectionDatabase(string server, string database, string login, string password)
        {

            if (login != "")
            {
                b_KnowLedge.Properties.Settings.Default.Reset();
                b_KnowLedge.Properties.Settings.Default.Servidor = server;
                b_KnowLedge.Properties.Settings.Default.BDName = database;
                b_KnowLedge.Properties.Settings.Default.SegInt = false;
                b_KnowLedge.Properties.Settings.Default.Login = login;
                b_KnowLedge.Properties.Settings.Default.Password = password;
                b_KnowLedge.Properties.Settings.Default.Save();
            }

            string providerName = "System.Data.SqlClient";
            string serverName = b_KnowLedge.Properties.Settings.Default.Servidor;
            string databaseName = b_KnowLedge.Properties.Settings.Default.BDName;
            bool security_integration = b_KnowLedge.Properties.Settings.Default.SegInt;
            string loginUser = b_KnowLedge.Properties.Settings.Default.Login;
            string pass = b_KnowLedge.Properties.Settings.Default.Password;


            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = loginUser;
            sqlBuilder.Password = pass;

            string providerString = sqlBuilder.ToString();

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            entityBuilder.Provider = providerName;

            entityBuilder.ProviderConnectionString = providerString;

            entityBuilder.Metadata = @"res://*/Models.BDKnowLedge.csdl|res://*/Models.BDKnowLedge.ssdl|res://*/Models.BDKnowLedge.msl";

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["BDKnowLedge"].ConnectionString = entityBuilder.ToString();
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");


            return this.Teste_Connection(entityBuilder);


        }

        public bool CreateDatabase()
        {
            bool done = true;
            List<string> script = new List<string>();
            string datasource = "";
            try
            {
                datasource = Properties.Settings.Default.Servidor;
            }
            catch { }

            string sqlConnectionString = "Data Source="+datasource+";Initial Catalog=master;Integrated Security=True";
            try
            {
                string path4;
                path4 = System.IO.Path.GetDirectoryName(
                   System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                path4 = path4 + "\\myscript.sql";
                path4 = path4.Replace("file:\\", "");
                string cmdText = ("SELECT SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1) FROM master.dbo.sysdatabases WHERE name = 'master'");
                string path = "";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                    {
                        var path1 = sqlCmd.ExecuteScalar();

                        path = Convert.ToString(path1);
                    }
                }

                string mdf = "N'" + path + b_KnowLedge.Properties.Settings.Default.BDName + "_data.mdf'";
                string ldf = "N'" + path + b_KnowLedge.Properties.Settings.Default.BDName + "_log.ldf'";

                string[] lines = System.IO.File.ReadAllLines(path4);
                string aux = "";
                foreach (string line in lines)
                {
                    aux = line;
                    aux = aux.Replace("@PathMdf", mdf);
                    aux = aux.Replace("@PathLdf", ldf);
                    aux = aux.Replace("@DatabaseName", b_KnowLedge.Properties.Settings.Default.BDName);
                    aux = Regex.Replace(aux, "([/*][*]).*([*][/])", " ");
                    script.Add(aux);
                }

                SqlConnection conn = new SqlConnection(sqlConnectionString);
                conn.Open();
                string query = "";
                foreach (string line in script)
                {
                    if (line == "GO")
                    {
                        Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(conn));
                        server.ConnectionContext.ExecuteNonQuery(query);
                        query = "";
                    }
                    else
                    {
                        query += " " + line;
                    }
                }
                conn.Close();
            }
            catch
            {
                done = false;
            }


            return done;
        }


        public string ChechDatabase()
        {
            String connString = ("Data Source=" + (b_KnowLedge.Properties.Settings.Default.Servidor + ";User ID=" + b_KnowLedge.Properties.Settings.Default.Login + ";Password=" + b_KnowLedge.Properties.Settings.Default.Password + ";"));
            string database = b_KnowLedge.Properties.Settings.Default.BDName;
            string cmdText = ("select count(*) from sys.databases where name=\'" + (database + "\';"));
            string bRet = "";

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch
                {
                    return bRet = "conn";
                }

                using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                {
                    var nRet1 = sqlCmd.ExecuteScalar();

                    int nRet = -1;

                    try
                    {
                        nRet = Convert.ToInt32(nRet1);
                    }
                    catch
                    { }

                    if (nRet <= 0)
                    {
                        bRet = "false";
                    }
                    else
                    {
                        bRet = "true";
                    }
                }
            }

            return bRet;
        }

        /// <summary>
        /// Faz um teste de conexão à Base de Dados.
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <remarks>
        /// Se a conexão falhar os settings são novamente preenchidos e é refeita a connectionString.
        /// </remarks>
        private bool Teste_Connection(EntityConnectionStringBuilder entityBuilder)
        {
            bool done = true;
            using (EntityConnection conn =
                new EntityConnection(entityBuilder.ToString()))
            {
                try
                {
                    conn.Open();
                    conn.Close();
                }
                catch
                {
                    done = false;
                    //this.ConnectionDatabase(b_KnowLedge.Properties.Settings.Default.Servidor, b_KnowLedge.Properties.Settings.Default.BDName,
                    //    b_KnowLedge.Properties.Settings.Default.Login, b_KnowLedge.Properties.Settings.Default.Password);

                }
            }

            return done;
        }

    }
}
