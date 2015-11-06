using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookieDatabase
{
    public class Users
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool EmailCanceled { get; set; }
        public virtual List<UserResult> UserResults { get; set; }
        public Users()
        {

        }


    }
}
