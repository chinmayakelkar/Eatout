using EatOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using Neo4jClient.Cypher;
namespace EatOut.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var model = new RestaurantListModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RestaurantListModel rest)
        {
            
            //search
            string rName = ("(?i).*") + (rest.RestaurantName == null ? "" : rest.RestaurantName) + (".*");
            string cuis = ("(?i).*") + (rest.Cuisine == null ? "" : rest.Cuisine) + (".*");
            string location = ("(?i).*") + (rest.Location == null ? "" : rest.Location) + (".*");
            string rating = (".*") + (rest.Rating <= 0.0 ? "" : rest.Rating.ToString()) + (".*");
            rating = (rest.Rating == 5.0 || rest.Rating == 5) ? rest.Rating.ToString() : rating; 
                var data = RouteConfig.GraphClient.Cypher.
                    Match("(r:Restaurant)-[s:Serves]->(c:Cuisine)," + "(r)-[loc:Located_At]->(l:Location)").Where("r.RestaurantName=~{r1}").WithParam("r1",rName).AndWhere("c.CuisineType=~{c1}").WithParam("c1",cuis).AndWhere("l.State=~{l1}").WithParam("l1",location).AndWhere("r.Rating=~{r2}").WithParam("r2",rating).ReturnDistinct((r, c, l) => new
                    {
                        rest = r.As<Restaurant>(),
                        cuis = c.As<Cuisine>(),
                        location = l.As<Location>(),
                    }).OrderByDescending("r.Rating");//.Limit(1000);

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
                int size = restResults.Count;
                return Json(new { data = restResults, iTotalRecords = size, iTotalDisplayRecords = size }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Recommendations()
        {
            var model = new RestaurantListModel();
            return View(model);
        }
        public ActionResult Favorites()
        {
            var model = new RestaurantListModel();
            return View(model);
        }

        public ActionResult MyRecommendations()
        {
            var model = new RestaurantListModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult MyRecommendations   (RestaurantListModel rest)
        {

            string rName = ("(?i).*") + (rest.RestaurantName == null ? "" : rest.RestaurantName) + (".*");
            string cuis = ("(?i).*") + (rest.Cuisine == null ? "" : rest.Cuisine) + (".*");
            string loct = ("(?i).*") + (rest.Location == null ? "" : rest.Location) + (".*");
            string rating = (".*") + (rest.Rating <= 0.0 ? "" : rest.Rating.ToString()) + (".*");
            rating = (rest.Rating == 5.0 || rest.Rating == 5) ? rest.Rating.ToString() : rating; 
            var user2 = User.Identity.GetUserName().ToString();
            
            var data2 = RouteConfig.GraphClient.Cypher.
               Match("(u:User)-[l:Likes]->(r:Restaurant), (r)-[s:Serves]->(c:Cuisine)<-[:Serves]-(r2:Restaurant), (r2)-[loc:Located_At]->(location:Location)")
               .Where("u.Email={p}")
               .WithParam("p", user2).AndWhere("r.LocationID=r2.LocationID").AndWhere("l.Rating>=3").AndWhere("r2.RestaurantName=~{rt}").WithParam("rt", rName).AndWhere("c.CuisineType=~{c2}").WithParam("c2", cuis).AndWhere("location.State=~{l2}").WithParam("l2", loct).AndWhere("r2.Rating=~{r3}").WithParam("r3",rating).ReturnDistinct((r2, c, location) => new
               {
                   rest = r2.As<Restaurant>(),
                   cuis = c.As<Cuisine>(),
                   location = location.As<Location>(),
               }).OrderByDescending("r2.Rating");


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
            int size = restResults.Count;
            return Json(new { data = restResults, iTotalRecords = size, iTotalDisplayRecords = size }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Favorites(RestaurantListModel rest)
        {

            string rName = ("(?i).*") + (rest.RestaurantName == null ? "" : rest.RestaurantName) + (".*");
            string cuis = ("(?i).*") + (rest.Cuisine == null ? "" : rest.Cuisine) + (".*");
            string loct = ("(?i).*") + (rest.Location == null ? "" : rest.Location) + (".*");
            string rating = (".*") + (rest.Rating <= 0.0 ? "" : rest.Rating.ToString()) + (".*");
            rating = (rest.Rating == 5.0 || rest.Rating == 5) ? rest.Rating.ToString() : rating; 

            var user2 = User.Identity.GetUserName().ToString();
            var data2 = RouteConfig.GraphClient.Cypher.
               Match("(u:User)-[l:Likes]->(r:Restaurant), (r)-[s:Serves]->(c:Cuisine), (r)-[loc:Located_At]->(location:Location)")
               .Where("u.Email={p}")
               .WithParam("p", user2).AndWhere("r.RestaurantName=~{r2}").WithParam("r2", rName).AndWhere("r.Rating=~{r3}").WithParam("r3",rating).AndWhere("c.CuisineType=~{c2}").WithParam("c2", cuis).AndWhere("location.State=~{l2}").WithParam("l2", loct).ReturnDistinct((r, c, location) => new
               {
                   rest = r.As<Restaurant>(),
                   cuis = c.As<Cuisine>(),
                   location = location.As<Location>(),
               }).OrderByDescending("r.Rating");

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
            int size = restResults.Count;
            return Json(new { data = restResults, iTotalRecords = size, iTotalDisplayRecords = size }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Recommendations(RestaurantListModel rest)
        {

            string rName = ("(?i).*") + (rest.RestaurantName == null ? "" : rest.RestaurantName) + (".*");
            string cuis = ("(?i).*") + (rest.Cuisine == null ? "" : rest.Cuisine) + (".*");
            string loct = ("(?i).*") + (rest.Location == null ? "" : rest.Location) + (".*");
            string rating = (".*") + (rest.Rating <= 0.0 ? "" : rest.Rating.ToString()) + (".*");
            rating = (rest.Rating == 5.0 || rest.Rating == 5) ? rest.Rating.ToString() : rating; 

            var user2 = User.Identity.GetUserName().ToString();
            var data2 = RouteConfig.GraphClient.Cypher.
               Match("(u:User)-[:Friend_Of]->(friend),(friend)-[l:Likes]->(r:Restaurant), (r)-[s:Serves]->(c:Cuisine), (r)-[loc:Located_At]->(location:Location)")
               .Where("u.Email={p}")
               .WithParam("p", user2).AndWhere("r.Rating>={val}").WithParam("val","3").AndWhere("r.RestaurantName=~{r2}").WithParam("r2", rName).AndWhere("r.Rating=~{r3}").WithParam("r3",rating).AndWhere("c.CuisineType=~{c2}").WithParam("c2", cuis).AndWhere("location.State=~{l2}").WithParam("l2", loct).ReturnDistinct((r, c, location) => new
               {
                   rest = r.As<Restaurant>(),
                   cuis = c.As<Cuisine>(),
                   location = location.As<Location>(),
               }).OrderByDescending("r.Rating");

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
            int size = restResults.Count;
            return Json(new { data = restResults, iTotalRecords = size, iTotalDisplayRecords = size }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Start()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {
            var model = new RestaurantModel();
            return View(model);
        }

        public ActionResult UsersList()
        {
            var model = new UserListModel();
            return View(model);
        }

        public ActionResult AddFriend(int id)
        {
            var user = User.Identity.GetUserName().ToString();
            var uID = id.ToString();
            var data1 = RouteConfig.GraphClient.Cypher.Match("(u2:User)").Where("u2.UserID={uid}").WithParam("uid",uID).ReturnDistinct((u2) => new
            {
                u = u2.As<Friend>()
            }).Limit(1);

            var data2=RouteConfig.GraphClient.Cypher.Match("(u1:User{Email:{e1}})-[f:Friend_Of]->(u2:User{UserID:{e2}})").WithParam("e1", user).WithParam("e2", uID).ReturnDistinct((u2) => new
            {
                u = u2.As<Friend>()
            }).Limit(1); 
            var friend = new UserListModel();
            foreach(var item in data1.Results){
                friend.Email = item.u.Email;
                friend.FirstName = item.u.FirstName;
                friend.LastName = item.u.LastName;
                friend.UserID = item.u.UserID;

                if (data2.Results.Count() == 0)
                {
                    friend.isFriend = false;
                }else if(data2.Results.Count()>0)
                {
                    friend.isFriend = true;
                }
            }
            return View(friend);
        }

        [HttpPost]
        public ActionResult AddFriend(UserListModel friend)
        {

            var user = User.Identity.GetUserName().ToString();
            var uId = friend.UserID.ToString();
            if (friend.isFriend)
            {
                RouteConfig.GraphClient.Cypher.Match("(u1:User{Email:{e1}}),(u2:User{UserID:{e2}})").WithParam("e1", user).WithParam("e2", uId).Merge("(u1)-[f:Friend_Of]->(u2)").ExecuteWithoutResults();
            }
            else
            {
                RouteConfig.GraphClient.Cypher.Match("(u1:User{Email:{e1}})-[f:Friend_Of]->(u2:User{UserID:{e2}})").WithParam("e1", user).WithParam("e2", uId).Delete("f").ExecuteWithoutResults();
            }
            
            return RedirectToAction("UsersList");
        }


        [HttpPost]
        public ActionResult UsersList(Friend f)
        {
            var email = ("(?i).*") + (f.Email == null ? "" : f.Email) + (".*");
            var first = ("(?i).*") + (f.FirstName == null ? "" : f.FirstName) + (".*");
            var last = ("(?i).*") + (f.LastName == null ? "" : f.LastName) + (".*");
            var user=User.Identity.GetUserName().ToString();

            var data1 = RouteConfig.GraphClient.Cypher.Match("(u1:User)-[f:Friend_Of]->(u2:User)").Where("u1.Email={e1}").WithParam("e1", user).AndWhere("u2.Email=~{z1}").WithParam("z1",email).AndWhere("u2.FirstName=~{z2}").WithParam("z2",first).AndWhere("u2.LastName=~{z3}").WithParam("z3",last).ReturnDistinct((u2) => new { 
                u=u2.As<Friend>()
            });
            List<Friend> friends = new List<Friend>();
            
            foreach (var item in data1.Results)
            {
                var friend = new Friend();
                friend.Email = item.u.Email;
                friend.FirstName = item.u.FirstName;
                friend.LastName = item.u.LastName;
                friend.UserID = item.u.UserID;
                friend.isFriend = true;
                friends.Add(friend);
            }

            var data2 = RouteConfig.GraphClient.Cypher.Match("(u1:User),(u2:User)").Where("u1.Email={e1}").WithParam("e1", user).AndWhere("NOT (u1)-[:Friend_Of]->(u2)").AndWhere("u2.Email=~{z1}").WithParam("z1",email).AndWhere("u2.FirstName=~{z2}").WithParam("z2",first).AndWhere("u2.LastName=~{z3}").WithParam("z3",last).AndWhere("u2.Email <>{zzz}").WithParam("zzz",user).ReturnDistinct((u2) => new
            {
                u = u2.As<Friend>()
            });

            foreach (var item in data2.Results)
            {
                var friend = new Friend();
                friend.Email = item.u.Email;
                friend.FirstName = item.u.FirstName;
                friend.LastName = item.u.LastName;
                friend.UserID = item.u.UserID;
                friend.isFriend = false;
                friends.Add(friend);
            }
            int size = friends.Count;
            return Json(new { data = friends, iTotalRecords=size, iTotalDisplayRecords=size }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Help()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
  
                string name=model.RestaurantName;
                string address=model.Address;
                string phone=model.Phone.ToString();

                //make restaurant node
               var data1= RouteConfig.GraphClient.Cypher.Match("(r:Restaurant)").With("COLLECT (r) AS restaurantNodes").
        Merge("(r:Restaurant{RestaurantName:{p1}, Address:{p2}, Phone:{p3}, Rating:{p4}})").WithParam("p1", name).WithParam("p2", address).WithParam("p3", phone).WithParam("p4", "0").OnCreate().Set("r.RestaurantID = toString(size(restaurantNodes)+1)").Return((r) => new
               {
                  
                   Id = Return.As<long>("r.RestaurantID")
               }).Results.FirstOrDefault();
        
                var Id=data1.Id.ToString();


                var cuisineType=model.CuisineType.ToString();
                var city=model.City.ToString();
                var state=model.State.ToString();

                //make cuisine node
                var data2 = RouteConfig.GraphClient.Cypher.Match("(cuisine:Cuisine)").With("COLLECT (cuisine) AS cuisineNodes").
                Merge("(c:Cuisine{CuisineType:{p1}})").WithParam("p1", cuisineType).OnCreate().Set("c.CuisineID = toString(size(cuisineNodes)+1)").Return(() => new
                {
                    Id = Return.As<long>("c.CuisineID")
                }).Results.FirstOrDefault();

                var cId = data2.Id.ToString();
                var pin = model.Pin.ToString();

                //make location node
                var data3 = RouteConfig.GraphClient.Cypher.Match("(location:Location)").With("COLLECT (location) AS locationNodes").
                Merge("(l:Location{Pin:{p1}})").WithParam("p1", pin).OnCreate().Set("l.LocationID = toString(size(locationNodes)+1), l.City = {p2},l.State = {p3}").WithParam("p2", city).WithParam("p3", state).Return(() => new
                {

                    Id = Return.As<long>("l.LocationID")
                }).Results.FirstOrDefault();


                var locId = data3.Id.ToString();
                
              
                //create relation r -> Serves -Cuisine
                RouteConfig.GraphClient.Cypher.Match("(r:Restaurant{RestaurantID:{p1}})").WithParam("p1",Id).OptionalMatch("(c:Cuisine{CuisineID:{p2}})").WithParam("p2",cId).With("r,c").
                Merge("(r)-[s:Serves]->(c)").OnCreate().Set("r.CuisineID = c.CuisineID").ExecuteWithoutResults();


                //create relation r -> Located_At - Location
                RouteConfig.GraphClient.Cypher.Match("(r:Restaurant{RestaurantID:{p1}})").WithParam("p1", Id).OptionalMatch("(l:Location{LocationID:{p2}})").WithParam("p2", locId).With("r,l").
                Merge("(r)-[s:Located_At]->(l)").OnCreate().Set("r.LocationID = l.LocationID").ExecuteWithoutResults();


            }

            return RedirectToAction("Index");
        }

        public ActionResult RateRestaurant(int id)
        {
            var user = User.Identity.GetUserName();
            string Id = id.ToString();
            Debug.WriteLine(Id);
            var data = RouteConfig.GraphClient.Cypher.Match("(u:User{Email:{email}}),(r:Restaurant{RestaurantID:{ID}})").
                WithParam("ID", Id).WithParam("email", user).Merge("(u)-[l:Likes]->(r)").OnCreate().Set("l.Rating=0").Return((r, l) => new
                {
                    rest = r.As<Rest>(),
                    rating=l.As<Rate>(),
                }).Limit(1);
            var restResult = new RestResult();
            foreach (var item in data.Results)
            {
                restResult.RestaurantName = item.rest.RestaurantName;
                restResult.RestaurantID = item.rest.RestaurantID;
                restResult.Address = item.rest.Address;
                restResult.Phone = item.rest.Phone;
                restResult.Rating = item.rating.Rating;
            }
            return View(restResult);
        }

        [HttpPost]
        public ActionResult RateRestaurant(RestResult restResult)
        {
            var user = User.Identity.GetUserName();
            string Id = restResult.RestaurantID.ToString();
            Debug.WriteLine(Id);
            int rating = (int)restResult.Rating;
            RouteConfig.GraphClient.Cypher.Match("(u:User{Email:{email}})-[l:Likes]->(r:Restaurant{RestaurantID:{ID}})").
                WithParam("ID", Id).WithParam("email", user).Set("l.Rating={rat}").WithParam("rat",rating).ExecuteWithoutResults();

            //update the overall rating of the restaurant

            RouteConfig.GraphClient.Cypher.Match("(u:User)-[l:Likes]->(r:Restaurant{RestaurantID:{ID}})").
               WithParam("ID", Id).With("r, toString(round(avg(l.Rating))) as average").Set("r.Rating=average").ExecuteWithoutResults();

            return RedirectToAction("Index");
        }
        
        public class Rest
        {
            public string RestaurantName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public long LocationID { get; set; }
            public string Links { get; set; }
            public long CuisineID { get; set; }
            public double Rating { get; set; }
            public long RestaurantID { get; set; }
        }

        public class Rate
        {
            public double Rating { get; set; }
        }
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

    public class Friend
    {
        public string Email { get; set; }
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public bool isFriend { get; set; }
    }
}