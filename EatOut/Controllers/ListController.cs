
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
namespace EatOut.Controllers
{
    
    public class ListController : Controller
    {
        // GET: List
        /// <summary>
        /// /MATCH (r)-[s:Serves]->(c:Cuisine), (r)-[loc:Located_At]->(location:Location)
        //RETURN DISTINCT r.RestaurantName, r.Address, r.Phone,c.CuisineType, location.City
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList()
        {

            var data = RouteConfig.GraphClient.Cypher.
                Match("(r:Restaurant)-[s:Serves]->(c:Cuisine)," + "(r)-[loc:Located_At]->(l:Location)").ReturnDistinct((r, c, l) => new
                {
                    rest = r.As<Restaurant>(),
                    cuis = c.As<Cuisine>(),
                    location = l.As<Location>(),
                }).Limit(1000);

            var restResults = new List<RestaurantResult>();
            
            foreach (var item in data.Results)
            {
                var restResult = new RestaurantResult();
                restResult.RestaurantID = item.rest.RestaurantID;
                restResult.RestaurantName = item.rest.RestaurantName;
                restResult.Phone = item.rest.Phone;
                restResult.Rating = item.rest.Rating;
                restResult.Address = item.rest.Address;
                restResult.Cuisine = item.cuis.CuisineType;
                restResult.Location = item.location.State;
                restResults.Add(restResult);
            }
            
            return Json(new { data = restResults}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecommendations()
        {
            var user2 = User.Identity.GetUserName().ToString();
            try
            {
               


                var data2 = RouteConfig.GraphClient.Cypher.
                Match("(u:User)-[:Friend_Of]->(friend),(friend)-[l:Likes]->(r:Restaurant), (r)-[s:Serves]->(c), (r)-[loc:Located_At]->(location)")
                .Where("u.Email={p}")
                .WithParam("p", user2).ReturnDistinct((r, c, location) => new
                {
                    rest = r.As<Restaurant>(),
                    cuis = c.As<Cuisine>(),
                    location = location.As<Location>(),
                }).Limit(20);

                var restResults = new List<RestaurantResult>();

                foreach (var item in data2.Results)
                {
                    var restResult = new RestaurantResult();
                    restResult.RestaurantID = item.rest.RestaurantID;
                    restResult.RestaurantName = item.rest.RestaurantName;
                    restResult.Phone = item.rest.Phone;
                    restResult.Rating = item.rest.Rating;
                    restResult.Address = item.rest.Address;
                    restResult.Cuisine = item.cuis.CuisineType;
                    restResult.Location = item.location.State;
                    restResults.Add(restResult);
                }

                return Json(new { data = restResults }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.GetBaseException());
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.HelpLink);
            }
            return View();
        }

        public class RestaurantResult   
        {
            public string RestaurantName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public long RestaurantID { get; set; }
            public double Rating { get; set; }
            public string Cuisine { get; set; }
            public string Location { get; set; }
        }

        public class Restaurant
        {
            public long RestaurantID { get; set; }
            public string RestaurantName { get; set; }
            public string Address { get; set; }
            public long LocationID { get; set; }
            public string Phone { get; set; }
            public string Links { get; set; }
            public long CuisineID { get; set; }
            public double Rating { get; set; }
        }

        public class Cuisine
        {
            public long CuisineID { get; set; }
            public string CuisineType { get; set; }
        }

        public class Location
        {
            public long LocationID { get; set; }
            public string Pin { get; set; }
            public string City { get; set; }
            public string State { get; set; }
        }
    }
}