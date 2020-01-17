using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenTube.Models
{
    public class VideoFiles
    {
        public int uploader { get; set; }
        public int videoID { get; set; }
        public string vidtitle { get; set; }
        public Nullable<int> videosize { get; set; }
        public string videopath { get; set; }
    }
}