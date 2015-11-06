using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookieDatabase
{
    public class BookieContext : DbContext
    {
        public BookieContext() :
            base("BookieContext")
        {

        }

        public DbSet<Matches> Matches { get; set; }
        public DbSet<UserResult> UserResults { get; set; }

        public DbSet<Users> Users { get; set; }
    }
}
