using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DatabaseLayer.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebLayer.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            UserManager<AppUser> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync(string fileType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            typeof(AppUser).GetProperties()
                            .Where(property => Attribute.IsDefined(property, typeof(PersonalDataAttribute)))
                            .ToList()
                            .ForEach(property =>
                            {
                                // convert bytes data to base 64 string type
                                // if (property.GetType() == typeof(Byte[]))
                                // {
                                //     var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(property.GetValue(user)?.ToString() ?? "null"));
                                //     personalData.Add(property.Name, base64String);
                                // }
                                // else
                                personalData.Add(property.Name, property.GetValue(user)?.ToString() ?? "null");
                            });
            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            if (fileType.Contains("JSON", StringComparison.InvariantCultureIgnoreCase))
            {
                Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
                return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
            }
            if (fileType.Contains("TEXT", StringComparison.InvariantCultureIgnoreCase))
            {
                Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.txt");
                var txtBuilder = new StringBuilder();
                personalData.ToList().ForEach(pair => txtBuilder.Append($"[{pair.Key}] - [{pair.Value}]\n"));
                return new FileContentResult(Encoding.UTF8.GetBytes(txtBuilder.ToString()), "text/plain");
            }
            if (fileType.Contains("CSV", StringComparison.InvariantCultureIgnoreCase))
            {
                Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.csv");
                var txtBuilder = new StringBuilder();
                personalData.ToList().ForEach(pair => txtBuilder.Append($"{pair.Key},{pair.Value}\n"));
                return new FileContentResult(Encoding.UTF8.GetBytes(txtBuilder.ToString()), "text/csv");
            }
            return Content("Not JSON FILE");
        }
    }
}
