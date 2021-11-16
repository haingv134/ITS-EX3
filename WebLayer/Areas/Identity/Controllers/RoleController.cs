// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Areas.Identity.Models.ManageViewModels;
using App.Areas.Identity.Models.RoleViewModels;
using WebLayer.ExtensionMethod;
using WebLayer.Models;
using ServicesLayer.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DatabaseLayer.Entity.Identity;
using DatabaseLayer.Context;
using DatabaseLayer.Entity;

namespace App.Areas.Identity.Controllers
{

    [Authorize(policy: "Manager")]
    [Area("Identity")]
    [Route("/Role/[action]")]
    public class RoleController : Controller
    {

        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DatabaseContext _context;
        private readonly UserManager<AppUser> _userManager;
        public RoleController(ILogger<RoleController> logger, RoleManager<IdentityRole> roleManager, DatabaseContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        // GET: /Role/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var r = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var roles = new List<RoleModel>();
            foreach (var role in r)
            {
                var claims = (await _roleManager.GetClaimsAsync(role)).Select(c => c.Type + "=" + c.Value).ToArray();
                roles.Add(new RoleModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Claims = claims
                });
            }
            return View(roles);
        }

        // GET: /Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // POST: /Role/Create
        [HttpPost, ActionName(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var newRole = new IdentityRole(model.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo role mới: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(string.Empty, error.Description));
            }
            return View();
        }

        // GET: /Role/Delete/roleid
        [HttpGet("{roleid}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            return View(role);
        }

        // POST: /Role/Delete/roleid
        [HttpPost("{roleid}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("Không tìm thấy role");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa: {role.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(string.Empty, error.Description));
            }
            return View(role);
        }

        [Authorize(policy: "AllowEditRole")]
        // GET: /Role/Edit/roleid
        [HttpGet("{roleid}"), ActionName("Edit")]
        public async Task<IActionResult> EditAsync(string roleid)
        {
            _logger.LogInformation("on edit role");
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var model = new EditRoleModel()
            {
                Name = role.Name,
                role = role,
                Claims = _context.RoleClaims.Where(rc => rc.RoleId == roleid).ToList()
            };

            return View(model);
        }

        // POST: /Role/Edit/1
        [HttpPost("{roleid}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmAsync(string roleid, [Bind("Name")] EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            //model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            model.role = role;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(string.Empty, error.Description));
            }

            return View(model);
        }


        [Authorize(policy: "AllowEditRole")]
        // GET: /Role/Edit/AddRoleClaim
        [HttpGet("{roleid}")]
        public async Task<IActionResult> AddRoleClaim(string roleid)
        {
            _logger.LogInformation("on edit role");
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var model = new EditClaimModel()
            {
                roleId = role.Id,
                roleName = role.Name                
            };

            return View(model);
        }
        [Authorize(policy: "AllowEditRole")]
        // GET: /Role/Edit/AddRoleClaim
        [HttpPost("{roleid}")]
        public async Task<IActionResult> AddRoleClaim(string roleid, EditClaimModel model)
        {
            _logger.LogInformation("on edit role claim");
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            bool isExistedClaim = (await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == model.ClaimType && c.Value == model.ClaimValue);
            if (isExistedClaim)
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có");
                return View(model);
            }
            else
            {
                var newClaim = new Claim(type: model.ClaimType, value: model.ClaimValue);
                var result = await _roleManager.AddClaimAsync(role, newClaim);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Lỗi không thể thêm claim cho role");
                    return View(model);
                }
                StatusMessage = "Đã thêm claim mơi thành công";
            }
            return RedirectToAction("Edit", new {roleid = roleid});
        }

        // GET: /Role/Edit/EditRoleClaim
        [HttpGet("{claimid}")]
        public async Task<IActionResult> EditRoleClaim(int? claimid)
        {
            _logger.LogInformation("on edit claim");
            if (claimid == null) return NotFound("Không tìm thấy claim");
            var claim = _context.RoleClaims.Where(rc => rc.Id == claimid).SingleOrDefault();
            if (claim == null) return Content("Không tìm thấy claims");
            var role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null ) return Content("Không tìm thâyd role");

            var model = new EditClaimModel(){
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
                roleId = role.Id,
                ClaimId = claimid.Value 
            };

            return View(model);
        }

        [HttpPost("{claimid}")]
        public async Task<IActionResult> EditRoleClaim(int? claimid, EditClaimModel model)
        {
            _logger.LogInformation("on edit claim");
            if (claimid == null) return NotFound("Không tìm thấy claim");
            var claim = _context.RoleClaims.Where(rc => rc.Id == claimid).SingleOrDefault();
            if (claim == null) return Content("Không tìm thấy claims");
            var role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null ) return Content("Không tìm thâyd role");

            bool isExistedClaim = _context.RoleClaims.Any(rc => rc.RoleId == role.Id &&  rc.ClaimType == model.ClaimType && rc.ClaimValue == model.ClaimValue);
            if (isExistedClaim)
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có");
                return View(model);
            }
            claim.ClaimType = model.ClaimType;
            claim.ClaimValue = model.ClaimValue;

            await _context.SaveChangesAsync();
            StatusMessage = "Vừa cập nhập thành công claims";

            return RedirectToAction(nameof(Index)) ;
        }

        [HttpPost("{claimid}")]
        public async Task<IActionResult> DeleteRoleClaim(int? claimid)
        {
            _logger.LogInformation("on edit claim");
            if (claimid == null) return NotFound("Không tìm thấy claim");
            var claim = _context.RoleClaims.Where(rc => rc.Id == claimid).SingleOrDefault();
            if (claim == null) return Content("Không tìm thấy claims");
            var role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null ) return Content("Không tìm thâyd role");

            await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));
            StatusMessage = "Vừa xóa thành công claims";

            return RedirectToAction(nameof(Index)) ;
        }
    }
}
