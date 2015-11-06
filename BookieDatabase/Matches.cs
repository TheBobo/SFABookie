using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookieDatabase
{
    public class Matches
    {
        [Key]
        public int Id { get; set; }

        public string Home { get; set; }
        public string Away { get; set; }
        public bool Close { get; set; }
        public int? HomeResult { get; set; }
        public int? AwayResult { get; set; }
        public DateTime TimeToBet { get; set; }

        public virtual List<UserResult> Results { get; set; }

        public Matches()
        {

        }
    }
}
