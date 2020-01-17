using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenTube.Models
{
    public class Creator : Controller
    {
        public List<VideoFile> myvids { get; set; }
        public int totalsubscribers { get; set; }
    }
}