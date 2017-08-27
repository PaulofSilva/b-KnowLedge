using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace b_KnowLedge.ViewModels
{
    class ImportDados
    {
        SqlConnectionStringBuilder connString2Builder = new SqlConnectionStringBuilder();

        public bool ConstroiConnString(string servidor, string baseDeDados, string userName, string password)
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
                        conn.Close();
                    }
                    catch
                    {
                        done = false;
                    }
                }
            }

            return done;
        }


        public DataTable ObtainDados(string query)
        {
            DataTable dt = null;

            using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();

                        command.CommandText = query;

                        SqlDataReader dr = command.ExecuteReader();
                        dt = new DataTable();
                        dt.Load(dr);

                        conn.Close();
                        
                    }
                    catch
                    {

                    }
                    finally
                    {
                        conn.Close();
                    }

                    return dt;
                }
            }
        }
    }
}
