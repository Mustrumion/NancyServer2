using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.Objects
{
    [Serializable]
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int ID { get; set; }
    }
}
