using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatOut.Models
{
    public class RestResult
    {
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double Rating { get; set; }
        public long RestaurantID { get; set; }
    }
}