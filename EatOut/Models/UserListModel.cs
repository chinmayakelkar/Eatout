using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatOut.Models
{
    public class UserListModel
    {
        public string Email { get; set; }
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isFriend { get; set; }
    }
}