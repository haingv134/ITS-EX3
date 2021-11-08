using System.Collections.Generic;
using DatabaseLayer.Entity.Identity;
using WebLayer.Models;

namespace App.Areas.Identity.Models.UserViewModels
{
        public class UserAndRole : AppUser
        {
            public string RoleNames { get; set; }        
        }
}