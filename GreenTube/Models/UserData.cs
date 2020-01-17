using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenTube.Models
{
    public class UserData
    {
        public int uid { get; set; }
        public int ucreator { get; set; }
        public string uemail { get; set; }
        public DateTime ubod { get; set; }
        public DateTime ujoin { get; set; }
    }
}