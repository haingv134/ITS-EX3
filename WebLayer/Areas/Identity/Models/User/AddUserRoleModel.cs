using System.Collections.Generic;
using System.ComponentModel;
using WebLayer.Models;
using DatabaseLayer.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Areas.Identity.Models.UserViewModels
{
  public class AddUserRoleModel
  {
    public AppUser user { get; set; }

    [DisplayName("Các role gán cho user")]
    public string[] RoleNames { get; set; }
  }
}