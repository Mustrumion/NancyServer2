using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.Objects
{
    public class Session
    {
        public User User { get; set; }
        public Guid SessionID { get; set; }
        public DateTime Expiration { get; set; }
    }
}
