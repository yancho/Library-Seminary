using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SeminaryLibrary.Account
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                LogOutMsg.Text = "You are now logged out! <br />If you would like to log in back again <a href='~/Account/Login.aspx' target='_self'>press here</a> ";

            }
            else
            {
                LogOutMsg.Text = "You are not logged in! <br />If you would like to log in to our site <a href='~/Account/Login.aspx' target='_self'>press here</a> ";
            }
        }


    }
}
