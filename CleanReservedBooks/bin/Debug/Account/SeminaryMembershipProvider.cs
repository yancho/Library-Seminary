using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using MySql.Web;
using MySql.Web.Security;

namespace TestLibrarySorting.Account
{
    public class SeminaryMembershipProvider : MembershipProvider
    {

        public SeminaryUsers SeminaryUsers
        {
            get;
            private set;
        }


        private int newPasswordLength = 8;
        private string connectionString;
        private string applicationName;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        public static int minRequiredNonAlphanumericCharacters;
        public static int minRequiredPasswordLength;
        private string passwordStrengthRegularExpression;

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }



        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }


        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser membershipUser = null;
            using (MySqlConnection sqlConnection = new MySqlConnection(connectionString))
            {
            
            try
            {

                sqlConnection.Open();


                MySqlCommand cmd = new MySqlCommand("select userfield0 as userId, userfield1 as title, userfield2 as Surname, userfield3 as Name, userfield4 as studentType, userfield17 as password, userfield18 as email from users where userfield18=?user", sqlConnection);

                cmd.Parameters.Add("?user", MySqlDbType.VarChar).Value = username;

               MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
               
               if (reader.HasRows)
               {
                   reader.Read();
                   membershipUser = GetUserFromReader(reader);
               }

            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            {
                sqlConnection.Close();
            }
            }
            return membershipUser;
        }


        public MembershipUser GetUserFromReader(
MySqlDataReader sqlDataReader
  )
        {

            object userID = sqlDataReader.GetValue(0);
            string title = sqlDataReader.GetString(1);
            string surname = sqlDataReader.GetString(2);
            string name = sqlDataReader.GetString(3);
            string studentType = sqlDataReader.GetString(4);
            string email = sqlDataReader.GetString(6);

            string passwordQuestion = String.Empty;
            if (sqlDataReader.GetValue(3) != DBNull.Value)
            {
                passwordQuestion = sqlDataReader.GetString(5);
            }


            MembershipUser membershipUser = new MembershipUser(this.Name, email, userID, title + " " + name + " " + surname, passwordQuestion, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
              

            return membershipUser;

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {

            if (!ValidateUser(username, oldPwd))
            {
                return false;
            }

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new Exception("Change password canceled due to new password validation failure.");
                }
            }

            MySqlConnection sqlConnection = new MySqlConnection(connectionString);
            MySqlCommand sqlCommand = new MySqlCommand("Update users SET userfield17=?newpass WHERE userfield18=?user ", sqlConnection);

            sqlCommand.Parameters.Add("?newpass", MySqlDbType.VarChar, 255).Value = newPwd;
            sqlCommand.Parameters.Add("?user", MySqlDbType.VarChar, 255).Value = username;
           

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //Add exception handling here.
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }

            return true;

        }

        public override bool ValidateUser(
   string username,
   string password
  )
        {
            bool isValid = false;
            using (MySqlConnection sqlConnection = new MySqlConnection(connectionString)) {
            
            try
            {

                sqlConnection.Open();


                MySqlCommand cmd = new MySqlCommand("select * from users where userfield18=?user AND userfield17=?pass", sqlConnection);

                cmd.Parameters.Add("?user", MySqlDbType.VarChar).Value = username;
                cmd.Parameters.Add("?pass", MySqlDbType.VarChar).Value = password;

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if ((count > 0))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            {
                sqlConnection.Close();
            }
}
            return isValid;
        }



        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {


            throw new NotImplementedException();

        }

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            
            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }

            return configValue;
        }


        public override void Initialize(string name, NameValueCollection config)
        {
                 
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "SeminaryMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "How Do I: Sample Membership provider");
            }

            
            //Initialize the abstract base class.

            base.Initialize(name, config);

            applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredAlphaNumericCharacters"], "0"));
            minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "5"));
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], String.Empty));
            enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            passwordFormat = MembershipPasswordFormat.Clear;

           

            ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if ((ConnectionStringSettings == null) || (ConnectionStringSettings.ConnectionString.Trim() == String.Empty))
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;

            //Get encryption and decryption key information from the configuration.
            System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            
        }

 
    }
}
