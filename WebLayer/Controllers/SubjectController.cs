using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicesLayer.Interface;
using Microsoft.Extensions.Logging;
using DatabaseLayer.Entity;
using DatabaseLayer.UnitOfWork;
using WebLayer.ViewModel.Subject;
using WebLayer.EditModel.Subject;
using AutoMapper;
using DatabaseLayer.ExceptionHandling;
using ServicesLayer.ViewModel;

namespace WebLayer.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ILogger<ClassController> logger;
        private readonly IClassServices classServices;
        private readonly ISubjectServices subjectServices;
        private readonly IMapper mapper;
        public string errorMessages { get; set; }
        public SubjectController(IClassServices classServices, ISubjectServices subjectServices, ILogger<ClassController> logger, IMapper mapper)
        {
            this.classServices = classServices;
            this.subjectServices = subjectServices;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index() => View();
        [HttpPost]
        public IActionResult SubjectList(int skip, int take)
        {
            var des = subjectServices.GetAll(skip, take);
            var count = subjectServices.GetCounting();
            return Json(new { success = true, data = des, totalRecords = count });
        }
        [HttpGet]
        public IActionResult Add() => View();
        [HttpPost]
        public IActionResult Add([Bind("Name")] AddEditModel source)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var subjectModel = mapper.Map<AddEditModel, Subject>(source);
                    subjectServices.AddSubject(subjectModel);
                    isSuccess = true;
                }
                catch (CustomeException e)
                {
                    errorMessages = e.Messages;
                }
            }
            else
            {
                errorMessages = "Input Form Invalid";
            }
            return Json(new { success = isSuccess, message = (isSuccess) ? "Add successfull" : errorMessages });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int subjectId)
        {
            bool isSuccess = false;
            try
            {
                await subjectServices.Delete(subjectId);
                isSuccess = true;
            }
            catch (CustomeException e)
            {
                errorMessages = e.Messages;
            }
            return Json(new { success = isSuccess, message = (isSuccess) ? "Delete successfull" : errorMessages });
        }
        [HttpGet]
        public IActionResult Edit(int subjectId)
        {
            try
            {
                var subjectModel = subjectServices.Get(subjectId);
                var subjectEditModel = mapper.Map<Subject, AddEditModel>(subjectModel);
                return View(subjectEditModel);
            }
            catch (CustomeException e)
            {
                return Content(e.Messages);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddEditModel subjectEditModel)
        {
            bool isSuccess = false;
            try
            {
                var subjectEdit = mapper.Map<AddEditModel, Subject>(subjectEditModel);
                await subjectServices.Update(subjectEdit);
                isSuccess = true;
            }
            catch (CustomeException e)
            {
                errorMessages = e.Messages;
            }
            return Json(new { success = isSuccess, message = (isSuccess) ? "Update successfull" : errorMessages });
        }
    }
}
