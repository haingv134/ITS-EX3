using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLayer.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebLayer.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            [Display(Name = "Tên đăng nhập")]
            public string Username { get; set; }

            [Display(Name = "Tên đầy đủ")]
            [StringLength(50, ErrorMessage = "{0} phải có chiều dài từ {2} tới {1}", MinimumLength = 3)]
            public string FullName { get; set; }
            public string Website { get; set; }
            [Phone]        
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Ngày Sinh")]
            public DateTime? Birthday { get; set; }
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Thông tin chi tiết")]
            public string About { get; set; }

        }

        private void Load(AppUser user)
        {
            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                FullName = user.FullName,
                Username = user.UserName,
                Website = user.Website,
                About = user.About,
                Birthday = user.Birthday
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            user.PhoneNumber = Input.PhoneNumber;
            // set other custome information
            user.FullName = Input.FullName;
            user.Address = Input.Address;
            user.Website = Input.Website;
            user.About = Input.About;
            user.Birthday = Input.Birthday;
            //
            var setOtherInformationResult = await _userManager.UpdateAsync(user);
            if (!setOtherInformationResult.Succeeded)
            {
                StatusMessage = "Unexpected error!";
                ModelState.AddModelError("abc", setOtherInformationResult.Errors.ToList()[0].ToString());
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
