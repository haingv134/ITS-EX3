using AutoMapper;
using DatabaseLayer.Entity;
using DatabaseLayer.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLayer.Interface;
using ServicesLayer.Interface.Datatable;
using ServicesLayer.ViewModel;
using ServicesLayer.ViewModel.DataTable;
using System.Linq;
using System.Threading.Tasks;
using WebLayer.EditModel.Class;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace WebLayer.Controllers
{
    //[Authorize(policy: "StudentManagement")]

    [Route("lop/[action]")]
    public class ClassController : Controller
    {
        private readonly ILogger<ClassController> logger;
        private readonly IClassServices classServices;
        private readonly IStudentServices studentServices;
        private readonly ISubjectServices subjectServices;
        private readonly IClassDtServices dataTableServices;
        private readonly IMapper mapper;
        private string errorMessages { get; set; }

        // inject Class Services to controller
        public ClassController(IClassServices classServices, IStudentServices studentServices, ISubjectServices subjectServices, IMapper mapper, IClassDtServices dataTableServices, ILogger<ClassController> logger)
        {
            this.classServices = classServices;
            this.studentServices = studentServices;
            this.subjectServices = subjectServices;
            this.dataTableServices = dataTableServices;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index() => View();
        [HttpPost]
        public IActionResult Index(DtParameters dtParameters, int maxValue, int minValue, string quantityType) => Json(dataTableServices.ResponseTable(dtParameters, minValue, maxValue, quantityType));

        [HttpGet("/themlop")]
        public IActionResult Add()
        {
            try
            {
                ViewBag.StudentList = studentServices.GetAvaiableStudent();
                ViewBag.SubjectList = subjectServices.GetAll();
                return View();
            }
            catch (CustomeException e)
            {
                return Content(e.Messages);
            }
        }

        [HttpPost("/themlop")]
        public async Task<IActionResult> Add(AddEditModel source)
        {
            bool isSuccess = false;
            // if all field is inputed correctly
            if (ModelState.IsValid)
            {
                try
                {
                    var model = mapper.Map<AddEditModel, ClassAddServicesModel>(source);
                    await classServices.AddClass(model);
                    isSuccess = true;
                }
                catch (CustomeException e)
                {
                    errorMessages = e.Messages;
                }
            }
            else errorMessages = $"Input form is invalid";
            return Json(new { success = isSuccess, message = (isSuccess) ? "Add successfull" : errorMessages });
        }

        [HttpGet]
        public IActionResult Edit(Guid classId)
        {
            try
            {
                var classEdit = classServices.GetClassEdit(classId);
                var data = mapper.Map<ClassEditServicesModel, EditEditModel>(classEdit);

                var studentList = studentServices.GetAvaibleStudentWithClass(classId);
                var selectedstudentList = studentServices.GetStudentListByClass(classId);
                data.StudentList = studentList.Select(student => new SelectListItem(){
                    Value = student.StudentId.ToString(),
                    Text = student.Name,
                    Selected = (selectedstudentList.Contains(student)) ? true : false
                }).ToList();

                var subjectList = subjectServices.GetAll();
                var selectedSubjectList = subjectServices.GetSubjectListByClass(classId);
                data.SubjectList = subjectList.Select(subject => new SelectListItem(){
                    Value = subject.SubjectId.ToString(),
                    Text = subject.Name,
                    Selected = (selectedSubjectList.Contains(subject)) ? true : false
                }).ToList();
                return View(data);
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
            // if all field is inputed correctly
            if (ModelState.IsValid)
            {
                var servicesModel = mapper.Map<EditEditModel, ClassEditServicesModel>(editModel);
                // update and return messages
                try
                {
                    await classServices.UpdateClass(servicesModel);
                    isSuccess = true;
                }
                catch (CustomeException e)
                {
                    errorMessages = e.Messages;
                }
            }
            else errorMessages = "Input form is invalid";
            return Json(new { success = isSuccess, message = isSuccess ? "Update class sucessfull" : errorMessages });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid classId)
        {
            bool isSuccess = false;
            try
            {
                await classServices.Delete(classId);
                isSuccess = true;
            }
            catch (CustomeException e)
            {
                errorMessages = e.Messages;
            }
            return Json(new { success = isSuccess, message = isSuccess ? "Delete class sucessfull" : errorMessages });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRange(string args)
        {
            List<Guid> classIdList = new List<Guid>();
            var spl = args.Split(",");
            var count = spl.Count();
            foreach (var classId in spl)
            {
                classIdList.Add(Guid.Parse(classId));
            }
            if (classIdList.Count() == 0) return Content("No Class Selected");

            bool isSuccess = false;
            try
            {
                await classServices.DeleteRange(classIdList.ToArray());
                isSuccess = true;
            }
            catch (CustomeException e)
            {
                errorMessages = e.Messages;
            }
            return Json(new { success = isSuccess, message = isSuccess ? "Delete class sucessfull" : errorMessages });
        }

        [HttpGet]
        public IActionResult AddSubject(string args)
        {

            List<Guid> classIdList = new List<Guid>();
            foreach (var classId in args.Split(","))
            {
                classIdList.Add(Guid.Parse(classId));
            }
            if (classIdList.Count() == 0) return Content("No Class Selected");
            try
            {
                var classListModel = classServices.GetAll();
                var selectedClassListModel = classServices.GetWithIDList(classIdList.ToArray());
                var classListItem = classListModel.Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.ClassId.ToString(),
                    Selected = (selectedClassListModel.Contains(cl)) ? true : false
                });

                ViewBag.SubjectList = subjectServices.GetAll().ToList();
                var addSubjectModel = new AddSubjectEditModel();
                addSubjectModel.ClassList = classListItem.ToList();

                return View(addSubjectModel);
            }
            catch (CustomeException e)
            {
                return Content(e.Messages);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(AddSubjectEditModel source)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var des = mapper.Map<AddSubjectEditModel, ClassAddSubjectServicesModel>(source);
                    await subjectServices.AddClassSubject(des);
                    isSuccess = true;
                }
                catch (CustomeException e)
                {
                    errorMessages = e.Messages;
                }
            }
            else errorMessages = "Modal Invalid";

            return Json(new { success = isSuccess, message = (isSuccess) ? "Update subject Successull" : errorMessages });
        }
    }
}
