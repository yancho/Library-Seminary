using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace CleanReservedBooks
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectString = ConfigurationManager.AppSettings["ConnectionString"];
            using (MySqlConnection connection = new MySqlConnection(connectString))
            {
                string sql = "delete  FROM reservelist where subdate(current_date,?daysToKeep) > STR_TO_DATE(date,'%d/%m/%Y')";

                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.Add("?daysToKeep", MySqlDbType.Int32).Value = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToKeep"]);
                connection.Open();
                try
                {
                    

                    int rowsDeleted = command.ExecuteNonQuery();
                    string path = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], DateTime.Now.ToString("dd-MM-yy") + ".log");
                    StreamWriter sw = new StreamWriter(path, true);
                    sw.WriteLine(string.Format("{0} : Deleted {1} rows",DateTime.Now.ToLongTimeString() ,rowsDeleted ));
                    sw.Flush();
                }
                catch (SqlException e)
                {
                    //Add exception handling here.

                }
                finally
                {
                    connection.Close();
                }

            }
        }
    }
}
