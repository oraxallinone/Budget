using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONSTRUCTION.Models
{
    public class trackingViewModel
    {
        public int slno { get; set; }
        public int trackName { get; set; }
        public int group1 { get; set; }
        public int group2 { get; set; }
        public int group3 { get; set; }
        public string isComplete { get; set; }
        public DateTime createdDate { get; set; }
    }
}