using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace SeminaryLibrary.Libraries
{
    internal class SemGetCurrentUser
    {
        internal static SemCurrentUser GetCurrentUser(string userEmail)
        {
            string username = "";
            string email = "";
            string userId = "";

            SemCurrentUser cu = new SemCurrentUser();

            using (MySqlConnection connection = Connection.GetDBConnection())
            {


                string sql = "SELECT  userfield1, userfield2, userfield3, userfield18, userfield0, userfield17, userfield4 FROM users WHERE userfield18 = ?userId";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.Add("?userId", MySqlDbType.VarChar).Value = userEmail;
                command.CommandType = CommandType.Text;

                MySqlDataReader sqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (sqlDataReader.Read())
                {
                    string title = sqlDataReader.GetString(0);
                    cu.Title = title;
                    string surname = sqlDataReader.GetString(1);
                    cu.Surname = surname;
                    string name = sqlDataReader.GetString(2);
                    cu.Name = name;
                    email = sqlDataReader.GetString(3);
                    cu.Email = email;
                    userId = sqlDataReader.GetString(4);
                    cu.UserId = userId;

                    username = title + " " + surname + ", " + name;
                    cu.UsernameFormatted = username;

                    string password = sqlDataReader.GetString(5);
                    
 
                    string publicAndPrivateKey;
                    string publicKey;

                    Encryption.GenerateKeys(SemCurrentUser.KeySize, out publicKey, out publicAndPrivateKey);

                    cu.PublicAndPrivateKey = publicAndPrivateKey;
                    cu.PublicKey = publicKey;

                    cu.Password = Encryption.EncryptText(password, SemCurrentUser.KeySize, cu.PublicKey);

                    string seminaryYr = sqlDataReader.GetString(6);
                    cu.SeminaryYr = seminaryYr;
                }

            }
            return cu;



        }
    }
}