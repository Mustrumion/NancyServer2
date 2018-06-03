using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.Objects
{
    public class UserPhoto
    {
        public int ProfileID { get; set; }
        public int UserID { get; set; }
        public string PhotoBase64 { get; set; }
    }
}
