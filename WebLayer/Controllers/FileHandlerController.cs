using AutoMapper;
using DatabaseLayer.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLayer.Interface;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.ExtensionMethod;
using Microsoft.AspNetCore.Authorization;

namespace WebLayer.Controllers
{
    [Authorize(policy: "StudentManagement")]
    public class FileHandlerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<FileHandlerController> _logger;
        private readonly IClassServices _classServices;
        private readonly IStudentServices _studentServices;

        public FileHandlerController(UserManager<AppUser> userManager, ILogger<FileHandlerController> logger, IClassServices classServices, IStudentServices studentServices)
        {
            _userManager = userManager;
            _logger = logger;
            _classServices = classServices;
            _studentServices = studentServices;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserImageProfile()
        {
            bool success = false;
            string message = string.Empty;

            var user = await _userManager.GetUserAsync(this.User);
            var form = HttpContext.Request.Form;
            if (user != null)
            {
                if (form.Files.Count == 1)
                {
                    var file = form.Files[0];
                    var bytes = new byte[file.Length];
                    using (var memory = new MemoryStream())
                    {
                        await file.CopyToAsync(memory);
                        bytes = memory.ToArray();
                        user.Image = bytes;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                message += error.Description + "\n";
                            }
                        }
                    }
                    success = true;
                    message = "Update avatar successfull";
                }
                else message = "Invalid file quanity";
            }
            else message = "User is not login!";
            return Json(new { success = success, message = message });
        }
        [Route("/tai-file/")]
        public IActionResult ExportClassDetailToCSV()
        {
            var details = _classServices.GetClassListDetail(string.Empty, 0, 0, 0, 0, "", out int record);
            Response.Headers.Add("Content-Disposition", "attachment; filename=ClassDetail.csv");
            return new FileContentResult(Encoding.UTF8.GetBytes(details.AsQueryable().CSVStringFormat()), "text/csv");
        }
        public IActionResult ExportStudentDetailToCSV()
        {
            var details = _studentServices.GetStudentListDetail(string.Empty,0, "", 0, 0, out int record);
            Response.Headers.Add("Content-Disposition", "attachment; filename=StudentDetail.csv");
            return new FileContentResult(Encoding.UTF8.GetBytes(details.AsQueryable().CSVStringFormat()), "text/csv");
        }
    }
}