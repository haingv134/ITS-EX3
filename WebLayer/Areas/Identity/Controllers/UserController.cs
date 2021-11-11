// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Areas.Identity.Models.AccountViewModels;
using App.Areas.Identity.Models.ManageViewModels;
using App.Areas.Identity.Models.RoleViewModels;
using App.Areas.Identity.Models.UserViewModels;
using WebLayer.ExtensionMethod;
using WebLayer.Models;
using ServicesLayer.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DatabaseLayer.Entity.Identity;
using DatabaseLayer.Context;

namespace App.Areas.Identity.Controllers
{

    //[Authorize(Roles = "Admin")]
    [Area("Identity")]
    [Route("/ManageUser/[action]")]
    public class UserController : Controller
    {

        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DatabaseContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(ILogger<RoleController> logger, RoleManager<IdentityRole> roleManager, DatabaseContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: /ManageUser/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await (_userManager.Users.OrderBy(user => user.UserName)).Select(user => new UserAndRole()
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            }).ToListAsync();

            foreach (var user in users)
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToArray<string>();
                user.RoleNames = string.Join(",", roles);
            }
            return View(users);
        }

        private async Task<List<IdentityRoleClaim<String>>>  GetUserClaimsInRoleAsync(string userId){
            var listRoleQuery = _context.UserRoles.Where(ur => ur.UserId == userId)
                                                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new {
                                                        userRole = ur,
                                                        role = r
                                                    }).Select(gr => gr.role);
            var listRoleClaimsQuery = listRoleQuery.Join(_context.RoleClaims, r => r.Id, rc => rc.RoleId, (r, rc) => new {
                role = r,
                roleClaim = rc
            }).Select(gr => gr.roleClaim);
            return await listRoleClaimsQuery.ToListAsync();
        }
        private async Task<List<IdentityUserClaim<string>>> GetUserClaimsAsync(string userId){
            return await _context.UserClaims.Where(uc => uc.UserId == userId).ToListAsync();                                             
        }
        // GET: /ManageUser/AddRole/id
        [HttpGet("{id}"), ActionName("AddRole")]
        public async Task<IActionResult> AddRoleAsync(string id)
        {
            // public SelectList allRoles { get; set; }
            var model = new AddUserRoleModel();
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            model.user = await _userManager.FindByIdAsync(id);
            if (model.user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }
            // lấy role mà user hiện tại đang có
            // select dựa vào model chứa tất cả rolename và dữ liệu truyền tới tới selectlist để render ra record nào selected
            model.RoleNames = (await _userManager.GetRolesAsync(model.user)).ToArray<string>();
            // lấy tất cả các role hiện tại trên hệ thống
            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);

            model.ListRoleClaims = await GetUserClaimsInRoleAsync(id);
            model.ListUserClaims = await GetUserClaimsAsync(id);

            return View(model);
        }

        // GET: /ManageUser/AddRole/id
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleAsync(string id, [Bind("RoleNames")] AddUserRoleModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            model.user = await _userManager.FindByIdAsync(id);

            if (model.user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }
            //await GetClaims(model);

            var OldRoleNames = (await _userManager.GetRolesAsync(model.user)).ToArray();

            var deleteRoles = OldRoleNames.Where(r => !model.RoleNames.Contains(r));
            var addRoles = model.RoleNames.Where(r => !OldRoleNames.Contains(r));

            // nếu add roles không thanhf công thì càn phải lấy lại dũe liêuc rồi send lại view
            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);

            var resultDelete = await _userManager.RemoveFromRolesAsync(model.user, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                ModelState.AddModelError(string.Empty, resultDelete.ToString());
                return View(model);
            }
            var resultAdd = await _userManager.AddToRolesAsync(model.user, addRoles);
            if (!resultAdd.Succeeded)
            {
                ModelState.AddModelError(string.Empty, resultAdd.ToString());
                return View(model);
            }
            StatusMessage = $"Vừa cập nhật role cho user: {model.user.UserName}";
            return RedirectToAction("Index");
        }
        //ManagerUser/SetPassword
        [HttpGet("{id}"), ActionName("SetPassword")]
        public async Task<IActionResult> SetPasswordAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }
            ViewBag.userName = user.UserName;
            return View();
        }

        //ManagerUser/SetPassword
        [HttpPost("{id}"), ActionName("SetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPasswordAsync(string id, SetUserPasswordModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userManager.RemovePasswordAsync(user);

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            StatusMessage = $"Vừa cập nhật mật khẩu cho user: {user.UserName}";

            return RedirectToAction("Index");
        }


        [HttpGet("{userid}"), ActionName("AddClaim")]
        public async Task<ActionResult> AddClaimAsync(string userid)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            ViewBag.user = user;
            return View();
        }

        [HttpPost("{userid}"),  ActionName("AddClaim")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddClaimAsync(string userid, AddUserClaimModel model)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            ViewBag.user = user;
            if (!ModelState.IsValid) return View(model);
            var claims = _context.UserClaims.Where(c => c.UserId == user.Id);

            if (claims.Any(c => c.ClaimType == model.ClaimType && c.ClaimValue == model.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính này đã có");
                return View(model);
            }

            await _userManager.AddClaimAsync(user, new Claim(model.ClaimType, model.ClaimValue));
            StatusMessage = "Đã thêm đặc tính cho user";

            return RedirectToAction("AddRole", new { id = user.Id });
        }

        [HttpGet("{claimid}")]
        public async Task<IActionResult> EditClaim(int claimid)
        {
            var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userclaim.UserId);

            if (user == null) return NotFound("Không tìm thấy user");

            var model = new AddUserClaimModel()
            {
                ClaimType = userclaim.ClaimType,
                ClaimValue = userclaim.ClaimValue
            };
            ViewBag.user = user;
            ViewBag.userclaim = userclaim;
            return View("AddClaim", model);
        }
        [HttpPost("{claimid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClaim(int claimid, AddUserClaimModel model)
        {
            var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userclaim.UserId);
            if (user == null) return NotFound("Không tìm thấy user");

            if (!ModelState.IsValid) return View("AddClaim", model);

            if (_context.UserClaims.Any(c => c.UserId == user.Id
                && c.ClaimType == model.ClaimType
                && c.ClaimValue == model.ClaimValue
                && c.Id != userclaim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có");
                return View("AddClaim", model);
            }


            userclaim.ClaimType = model.ClaimType;
            userclaim.ClaimValue = model.ClaimValue;

            await _context.SaveChangesAsync();
            StatusMessage = "Bạn vừa cập nhật claim";


            ViewBag.user = user;
            ViewBag.userclaim = userclaim;
            return RedirectToAction("AddRole", new { id = user.Id });
        }
        [HttpPost("{claimid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClaimAsync(int claimid)
        {
            var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userclaim.UserId);

            if (user == null) return NotFound("Không tìm thấy user");

            await _userManager.RemoveClaimAsync(user, new Claim(userclaim.ClaimType, userclaim.ClaimValue));

            StatusMessage = "Bạn đã xóa claim";

            return RedirectToAction("AddRole", new { id = user.Id });
        }
    }
}
