using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatOut.Models
{
    public class RestaurantListModel
    {
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Cuisine { get; set; }
        public string Location { get; set; }
        public double Rating { get; set; }
    }
}