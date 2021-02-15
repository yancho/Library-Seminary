using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeminaryLibrary.Libraries
{
    internal class SemCurrentUser
    {
        internal string UserId { get; set; }
        internal string Title { get; set; }
        internal string Surname { get; set; }
        internal string Name { get; set; }
        internal string SeminaryYr { get; set; }
        internal string Password { get; set; }
        internal string Email { get; set; }
        internal string UsernameFormatted { get; set; }
        internal string PublicKey { get; set; }
        internal string PublicAndPrivateKey { get; set; }
        internal const int KeySize = 1024;

    }


}