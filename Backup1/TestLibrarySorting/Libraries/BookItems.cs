using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SeminaryLibrary.Libraries
{
    public static class BookItems
    {
        public static bool CheckSelectedItems(CheckBoxList cbList, int maxBooks)
        {
            int count = GetReservedBooks(cbList, maxBooks);
            if (count > maxBooks)
            { return true; }
            else return false;
        }

        public static int GetReservedBooks(CheckBoxList cbList, int maxBooks)
        {
            int count = 0;
            for (int i = 0; cbList.Items.Count > i; i++)
            {
                if (cbList.Items[i].Selected)
                    count++;
            }
            return count;
        }

        public static bool CheckIfAnItemIsAtLeastSelected(CheckBoxList cbList)
        {
            int count = 0;
            for (int i = 0; cbList.Items.Count - 1 > i; i++)
            {
                if (cbList.Items[i].Selected)
                    count++;
            }
            if (count >= 1)
            { return true; }
            else return false;
        }

        internal static int ReserveTheseBooksToTheCurrentUser(CheckBoxList cbList, string userId)
        {
            int count = 0;

            List<string> BooksBooked = new List<string>();

            SemCurrentUser cu = SemGetCurrentUser.GetCurrentUser(userId);

            for (int i = 0; i <= cbList.Items.Count - 1; i++)
            {
                if (cbList.Items[i].Selected)
                {
                    ReserveABookToTheCurrentUser(cu, RegexTheName(cbList.Items[i].Value));
                    BooksBooked.Add(cbList.Items[i].Value);
                    count++;
                }
            }
            //sending email to staff
            EmailLibrary.ReserveBooks(BooksBooked, cu, count, ConfigurationManager.AppSettings["libraryToAddress"], ConfigurationManager.AppSettings["libraryFromAddress"], ConfigurationManager.AppSettings["mailSrvUser"], ConfigurationManager.AppSettings["mailSrvPass"], ConfigurationManager.AppSettings["mailSrvIP"]);

             return count; 
        }

        private static string RegexTheName(string value)
        {
            Match match = Regex.Match(value, @"([^\s]+)",
	    RegexOptions.IgnoreCase);

	// Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match.Groups[1].Value;
                return key;
            }
            else return String.Empty;
        }

        internal static int GetNumberOfAlreadyReservedBooks(string userId)
        {

            int reservedBooksAlready = 0;

            SemCurrentUser cu = SemGetCurrentUser.GetCurrentUser(userId);

            using (MySqlConnection connection = Connection.GetDBConnection())
            {
                string sql = "SELECT COUNT(*) from reservelist where userid = ?userId";

                MySqlCommand command = new MySqlCommand(sql, connection);
               
                command.Parameters.Add("?userId", MySqlDbType.VarChar).Value = cu.UserId;
                
                command.CommandType = CommandType.Text;

                try
                {

                   reservedBooksAlready = Convert.ToInt32(command.ExecuteScalar());
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
            return reservedBooksAlready;
        }

        private static void ReserveABookToTheCurrentUser(SemCurrentUser cu, string itemId)
        {
            //insert into reservelist (date,time,seconds,itemid,userid,username,status,dispnum,units) values ('22/05/2012','11:54','3.420532e+9','F00367','1234','Mr Gove, Jean','Res','0','1')

            //correcting the username

            

            using (MySqlConnection connection = Connection.GetDBConnection())
            {
                string sql = "INSERT into qtracklog.reservelist (date,time,seconds,itemid,userid,username,status,dispnum,units) VALUES (?date, ?time, ?seconds, ?itemid, ?userId, ?userName, 'Res', '0', '1')";
                   
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.Add("?date", MySqlDbType.VarChar).Value = DateTime.Now.Date.ToShortDateString();
                command.Parameters.Add("?time", MySqlDbType.VarChar).Value = DateTime.Now.ToLocalTime().ToShortTimeString();
                command.Parameters.Add("?seconds", MySqlDbType.VarChar).Value = DateTime.Now.Second;
                command.Parameters.Add("?itemId", MySqlDbType.VarChar).Value = itemId;
                command.Parameters.Add("?userId", MySqlDbType.VarChar).Value = cu.UserId;
                command.Parameters.Add("?userName", MySqlDbType.VarChar).Value = cu.UsernameFormatted;
                command.CommandType = CommandType.Text;

                try
                {
                    
                    command.ExecuteNonQuery();
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