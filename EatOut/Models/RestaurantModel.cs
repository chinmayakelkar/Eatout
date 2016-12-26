using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatOut.Models
{
    public class RestaurantModel
    {
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Link { get; set; }
        public string CuisineType { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}