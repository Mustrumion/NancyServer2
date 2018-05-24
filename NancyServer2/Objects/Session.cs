using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.Objects
{
    public class Token
    {
        public Guid SessionID { get; set; }
        public DateTime Expiration { get; set; }
        public User User { get; set; }
    }
}
