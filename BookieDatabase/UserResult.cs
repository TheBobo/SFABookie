using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookieDatabase
{
    public class UserResult
    {
        public int Id { get; set; }
        public int? Home { get; set; }
        public int? Away { get; set; }
        public bool ResultConfirm { get; set; }
        public bool GivenMoney { get; set; }
        public bool GivenMoneyByBookie { get; set; }
        public bool ComfirmResult { get; set; }

        public int MatchId { get; set; }
        public Matches Match { get; set; }

        public int UserId { get; set; }
        public virtual Users User { get; set; }

        public UserResult()
        {

        }
    }
}
