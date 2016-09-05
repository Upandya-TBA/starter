using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateReader
{
    public class DB_Hepler
    {
        private const int BUFFER_SIZE = 10240;

        //private const string testDatabase = "SQLOLEDB.1; SERVER=beasttestnu3; DATABASE=sessionserver;";
        //private const string demoDatabase = "SQLOLEDB.1; SERVER=10.75.4.100; DATABASE=sessionserver;";
        //private const string productionDatabase = "SQLOLEDB.1; SERVER=172.30.5.110; DATABASE=sessionserver;";

        public SqlCommand command = null;
        public SqlDataReader dataReader = null;

        #region Database connection according to environment
        private static SqlConnection Connect(string environment)
        {
            SqlConnection connection = null;
            SqlConnectionStringBuilder dbConnectionStr = null;

            switch (environment)
            {
                case "TEST":
                    dbConnectionStr = new SqlConnectionStringBuilder();
                    dbConnectionStr.InitialCatalog = "sessionserver";
                    dbConnectionStr.DataSource = "192.168.33.209";
                    dbConnectionStr.UserID = "rradmin";
                    dbConnectionStr.Password = "rradmin";

                    connection = new SqlConnection(dbConnectionStr.ConnectionString);
                    break;

                case "DEMO":
                    dbConnectionStr = new SqlConnectionStringBuilder();
                    dbConnectionStr.InitialCatalog = "sessionserver";
                    dbConnectionStr.DataSource = "10.75.4.100";
                    dbConnectionStr.UserID = "rradmin";
                    dbConnectionStr.Password = "rradmin";

                    connection = new SqlConnection(dbConnectionStr.ConnectionString);
                    break;

                case "PRODUCTION":
                    dbConnectionStr = new SqlConnectionStringBuilder();
                    dbConnectionStr.InitialCatalog = "sessionserver";
                    dbConnectionStr.DataSource = "172.30.5.110";
                    dbConnectionStr.UserID = "rradmin";
                    dbConnectionStr.Password = "rradmin";

                    connection = new SqlConnection(dbConnectionStr.ConnectionString);
                    break;
            }
            return connection;
        }   // END OF Connection(string environment)
        #endregion

        /// <summary>
        /// Get all product list
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public DataTable GetAllProduct(string environment)
        {
            string sqlQuery = @"SELECT ObjectID, UPPER(ObjectName) as Object_Name 
                                    FROM objectName WHERE [objectName] 
                                        Like '%Addin%' OR objectName Like '%BeastExcel%' 
                                        order by ObjectName";

            SqlConnection con = Connect(environment);
            command = new SqlCommand(sqlQuery, con );

            if (con.State == ConnectionState.Open) con.Close();
            con.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }


    }
}
