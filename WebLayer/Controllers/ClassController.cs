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
namespace WebLayer.Controllers
{
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
        public IActionResult Add() => View();

        [HttpPost("/themlop")]
        public JsonResult Add([Bind("Name")] AddEditModel source)
        {
            bool isSuccess = false;
            // if all field is inputed correctly
            if (ModelState.IsValid)
            {
                try
                {
                    // map AddClassEditModel -> ClassModel
                    var classModel = mapper.Map<AddEditModel, ClassModel>(source);
                    classServices.AddClass(classModel);
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
        public IActionResult Edit(int classId)
        {
            try
            {
                var classEdit = classServices.GetClassEdit(classId);
                var data = mapper.Map<ClassEditServicesModel, EditEditModel>(classEdit);
                var studentList = studentServices.GetStudentListByClass(classId).ToList();
                ViewBag.StudentList = studentList;
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
        public async Task<IActionResult> Delete(int classId)
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

        [HttpGet]
        public IActionResult AddSubject()
        {
            ViewBag.ClassList = classServices.GetAll().ToList();
            ViewBag.SubjectList = subjectServices.GetAll().ToList();
            return View();
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
            } else errorMessages = "Modal Invalid";

            return Json(new { success = isSuccess, message = (isSuccess) ? "Update subject Successull" : errorMessages });
        }
    }
}
