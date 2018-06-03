using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.Objects
{
    public class UserProfile
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public bool NameVisible { get; set; }
        public string Surname { get; set; }
        public bool SurnameVisible { get; set; }
        public string Gender { get; set; }
        public bool GenderVisible { get; set; }
        public string Nick { get; set; }
        public string Interests { get; set; }
        public bool InterestsVisible { get; set; }
        public string Description { get; set; }
        public bool DescriptionVisible { get; set; }
        public DateTime Born { get; set; }
        public bool AgeVisible { get; set; }
    }
}
