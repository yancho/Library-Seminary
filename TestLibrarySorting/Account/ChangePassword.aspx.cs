using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SeminaryLibrary.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        MembershipUser thisUser;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                thisUser = Membership.GetUser();
                name.Text = thisUser.Email;
                id.Text = Convert.ToString(thisUser.ProviderUserKey);
                email.Text = thisUser.UserName;
            }
        }

        protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx", false);
            }
            else
            {
                thisUser = Membership.GetUser();
                SeminaryMembershipProvider custom = (SeminaryMembershipProvider)Membership.Providers["SeminaryMembershipProvider"];

                bool result = custom.ChangePassword(thisUser.UserName, ChangeUserPassword.CurrentPassword, ChangeUserPassword.NewPassword);
            }

        }
    }
}
