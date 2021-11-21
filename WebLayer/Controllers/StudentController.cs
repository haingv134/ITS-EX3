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
using Microsoft.AspNetCore.Authorization;

namespace WebLayer.Controllers
{
    //[Authorize(policy: "StudentManagement")]
    public class StudentController : Controller
    {
        private readonly ILogger<ClassController> logger;
        private readonly IStudentServices studentServices;
        private readonly ISubjectServices subjectServices;
        private readonly IClassServices classServices;
        private readonly IStudentDtServices studentDtServices;
        private readonly IMapper mapper;
        private string errorMessages { get; set; }
        public StudentController(
            IStudentServices studentServices,
            IClassServices classServices,
            IStudentDtServices studentDtServices,
            ISubjectServices subjectServices,
            ILogger<ClassController> logger, IMapper mapper)
        {
            this.logger = logger;
            this.studentServices = studentServices;
            this.classServices = classServices;
            this.studentDtServices = studentDtServices;
            this.subjectServices = subjectServices;
            this.mapper = mapper;
        }
        public IActionResult Index(string keysearch)
        {
            try
            {
                var classList = classServices.GetAll();
                ViewBag.ClassList = classList;
                if (classList.Count == 0) return Content("No class found, a student must be assign to at least one class");
                return View((object)keysearch);
            }
            catch (CustomeException e)
            {
                return Content(e.Messages);
            }
        }
        [HttpPost]
        public IActionResult Index(DtParameters dtParameters, string gender, Guid classid) => Json(studentDtServices.ResponseTable(dtParameters, gender, classid));

        [HttpPost]
        public IActionResult GetSubjectListByStudent(Guid studentId)
        {
            var res = subjectServices.GetSubjectListByStudent(studentId);
            return Json(new { data = res });
        }
        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var classList = classServices.GetAll().ToList();
                ViewBag.ClassList = classList;
                if (classList.Count == 0) return Content("No class found, can not add student if has no class");
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
        public IActionResult Edit(Guid studentId)
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
        public async Task<IActionResult> Delete(Guid studentId)
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
