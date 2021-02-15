using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SeminaryLibrary.Libraries
{
    public class Connection
    {
        //public static SqlConnection GetDBConnection()
        //{

        //    ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings["testLibraryConnectionString"];

        //    SqlConnection connection = new SqlConnection(ConnectionStringSettings.ConnectionString);


        //    connection.Open();
        //    return connection;
        //}

        public static MySqlConnection GetDBConnection()
        {

            ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings["MySqlLibrary"];

            MySqlConnection connection = new MySqlConnection(ConnectionStringSettings.ConnectionString);


            connection.Open();
            return connection;
        }
    }
}