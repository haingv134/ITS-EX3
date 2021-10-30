using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicesLayer.Interface;
using Microsoft.Extensions.Logging;
using DatabaseLayer.Entity;
using DatabaseLayer.UnitOfWork;
using DatabaseLayer.Enum;
using WebLayer.ViewModel.Student;
using WebLayer.EditModel.Student;
using AutoMapper;
using ServicesLayer.ViewModel;
using DatabaseLayer.ExceptionHandling;
using ServicesLayer.Interface.Datatable;
using ServicesLayer.ViewModel.DataTable;

namespace WebLayer.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<ClassController> logger;
        private readonly IStudentServices studentServices;
        private readonly IClassServices classServices;
        private readonly IStudentDtServices studentDtServices;
        private readonly IMapper mapper;
        private string errorMessages { get; set; }
        public StudentController(IStudentServices studentServices, IClassServices classServices, IStudentDtServices studentDtServices, ILogger<ClassController> logger, IMapper mapper)
        {
            this.logger = logger;
            this.studentServices = studentServices;
            this.classServices = classServices;
            this.studentDtServices = studentDtServices;
            this.mapper = mapper;
        }
        public IActionResult Index() => View();
        [HttpPost]
        //public IActionResult Search(string keyword) => Json(studentDtServices.ResponseTable(keyword);
        public IActionResult Index(DtParameters dtParameters, bool gender) => Json(studentDtServices.ResponseTable(dtParameters, gender));

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var classList = classServices.GetAll().ToList();
                ViewBag.ClassList = classList;
                if (classList.Count == 0 ) return Content("No class found, can not add student if has no class");
            }
            catch (CustomeException e)
            {
                return Content(e.Messages);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddEditModel source)
        {
            logger.LogInformation($"Adding new student with name: {source.Name}");
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var addStudentServicesModel = mapper.Map<AddEditModel, StudentAddServicesModel>(source);
                    studentServices.AddStudent(addStudentServicesModel);
                    isSuccess = true;
                }
                catch (CustomeException e)
                {
                    errorMessages = e.Messages;
                }
            }
            else errorMessages = "Input form is invalid";
            return Json(new { success = true, message = (isSuccess ? "Add successfull" : errorMessages) });
        }        
        [HttpGet]
        public IActionResult Edit(int studentId)
        {
            try
            {
                var studentEdit = studentServices.GetStudentEdit(studentId);
                var editModel = mapper.Map<StudentEditServicesModel, EditEditModel>(studentEdit);
                ViewBag.ClassList = classServices.GetAll().ToList();
                return View(editModel);
            }
            catch (CustomeException e)
            {
                return Content(e.Messages);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditEditModel editModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var servicesModel = mapper.Map<EditEditModel, StudentEditServicesModel>(editModel);
                    await studentServices.UpdateStudent(servicesModel);
                    isSuccess = true;
                }
                catch (CustomeException e)
                {
                    errorMessages = e.Messages;
                }
            }
            else errorMessages = "Invalid input form";
            return Json(new { success = isSuccess, message = isSuccess ? " Update Successfull" : errorMessages });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int studentId)        
        {
            bool isSuccess = false;
            try
            {
                await studentServices.Delete(studentId);
                isSuccess = true;
            }
            catch (CustomeException e)
            {
                errorMessages = e.Messages;
            }
            return Json(new { success = isSuccess, message = isSuccess ? "Delete successful" : errorMessages });
        }

        public IActionResult Oldest()
        {
            ViewBag.Infor = "Oldest Student";
            var oldest = mapper.Map<StudentOldServicesModel[], OldViewModel[]>(studentServices.GetOldestStudent().ToArray());
            return View("Old", oldest);
        }

        public IActionResult Youngest()
        {
            ViewBag.Infor = "Youngest Student";
            var youngest = mapper.Map<StudentOldServicesModel[], OldViewModel[]>(studentServices.GetYoungestStudent().ToArray());
            return View("Old", youngest);
        }
    }
}
