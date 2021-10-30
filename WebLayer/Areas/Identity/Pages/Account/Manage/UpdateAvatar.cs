using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLayer.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebLayer.Areas.Identity.Pages.Account.Manage
{
    public class UpdateAvatar
    {        
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UpdateAvatar> _logger;
        public UpdateAvatar(
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor, 
            ILogger<UpdateAvatar> logger)
            {
                _userManager = userManager;
                _logger = logger;
                _httpContextAccessor = httpContextAccessor;
            }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> FormHander(HttpContext context)
        {            
            bool success = false;
            var identityUser = _httpContextAccessor.HttpContext?.User;
            var user =  await _userManager.GetUserAsync(identityUser);
            var form = context.Request.Form;
            if (user != null)
            {
                if (form.Files.Count != 1)
                {
                    var file = form.Files[0];
                    var bytes = new byte[file.Length];
                    using (var memory = new MemoryStream())
                    {
                        await file.CopyToAsync(memory);
                        bytes = memory.ToArray();
                        user.Image = bytes;
                        //var result = await _userManager.UpdateAsync(user);
                    }
                    success = true;
                    _logger.LogInformation("File founded")                    ;
                }
                else
                {
                    StatusMessage = "Invalid file quanity";
                    success = false;
                }
            } else {
                StatusMessage = "Unexpected error occured";
                success = false;
            }
            return new JsonResult(new { success = success, message = StatusMessage });
        }
    }
}