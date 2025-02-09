using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONSTRUCTION.Models
{
    public class tblBudget_MV
    {
        public int id { get; set; }
        public string details { get; set; }
        public Nullable<decimal> price { get; set; }
        public string createdDate { get; set; }
    }
}