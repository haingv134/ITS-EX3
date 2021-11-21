using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebLayer.Models;
using ServicesLayer.Interface;
using DatabaseLayer.Entity;
using DatabaseLayer.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft;
using Microsoft.AspNetCore.Authorization;

namespace WebLayer.Controllers
{
    //[Authorize(policy: "StudentManagement")]
    public class HomeController : Controller
    {
        private readonly IClassServices classServices;

        public HomeController(IClassServices classServices)
        {
            this.classServices = classServices;
        }

        public IActionResult Index()
        {
            return View();
        }        
        public IActionResult GenerateJson()
        {
            var allData = new AllData()
            {
                Classes = classServices.GetAll().ToList(),
            };
            ViewBag.jsonstring = JsonConvert.SerializeObject(allData, Formatting.Indented, new JsonSerializerSettings() {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
