using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestLibrarySorting.Account
{
    public class SeminaryUsers
    {
        public SeminaryUsers() { }

        public SeminaryUsers(string email, string password, string fullName)
        {
            UserId = email;
            Password = password;
            FullName = fullName;
        }
        public virtual string UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Password { get; set; }
    }
}