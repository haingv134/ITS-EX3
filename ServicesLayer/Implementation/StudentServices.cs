using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.Interface;
using DatabaseLayer.UnitOfWork;
using DatabaseLayer.Entity;
using Microsoft.Extensions.Logging;
using ServicesLayer.ViewModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.ExceptionHandling;
using DatabaseLayer.Enum;

namespace ServicesLayer.Implementation
{
    public class StudentServices : IStudentServices
    {
        private readonly UnitOfWork unitOfWork;

        public StudentServices(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void AddStudent(StudentAddServicesModel studentServicesModel)
        {
            var studentModel = studentServicesModel.GetStudent();
            var classStudentModel = studentServicesModel.GetClassStudent();
            try
            {
                // insert and get last studentModel 
                var student = unitOfWork.StudentRepository.Insert(studentModel, true);
                // add to connection between class - student
                classStudentModel.StudentId = student.StudentId;
                unitOfWork.ClassStudentRepository.Insert(classStudentModel, true);
            }
            catch (CustomeException e)
            {
                throw new CustomeException($"Add Student With name: {studentModel.Name} unsuccessful, Error: {e.Message}");
            }
        }
        public Student Get(int id) => unitOfWork.StudentRepository.Get(id) ?? throw new CustomeException("Null student object");
        public async Task Delete(int id)
        {
            try
            {
                var studentModel = Get(id);
                unitOfWork.StudentRepository.Remove(studentModel);
                int roweffected = await unitOfWork.SaveChange();
                if (roweffected == 0) throw new CustomeException("No effected in database");
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<Student> GetAvaiableStudent() => unitOfWork.StudentRepository.GetAvaibleStudent().ToList();
        public List<Student> GetAvaibleStudentWithClass(int classid) => unitOfWork.StudentRepository.GetAvaibleStudentWithClass(classid).ToList();
        public List<Student> GetAll() => unitOfWork.StudentRepository.GetAll().ToList();
        public int GetCounting() => unitOfWork.StudentRepository.GetCounting();
        public List<Student> GetStudentListByClass(int classId) => unitOfWork.StudentRepository.GetStudentListbyClass(classId).ToList();
        public List<StudentOldServicesModel> GetYoungestStudent()
        {
            return unitOfWork.StudentRepository.GetYoungestStudent().ToList().Select(student => new StudentOldServicesModel()
            {
                Name = student.Name,
                StudentId = student.StudentId,
                Old = DateTime.Now.Year - student.Birthday.Year
            }).ToList();
        }
        public List<StudentOldServicesModel> GetOldestStudent()
        {
            return unitOfWork.StudentRepository.GetOldestStudent().ToList().Select(student => new StudentOldServicesModel()
            {
                Name = student.Name,
                StudentId = student.StudentId,
                Old = DateTime.Now.Year - student.Birthday.Year
            }).ToList();
        }
        public StudentEditServicesModel GetStudentEdit(int studentId)
        {
            try
            {
                var studentModel = unitOfWork.StudentRepository.GetStudentEdit(studentId) ?? throw new CustomeException("Null student object");
                // get require information about current student
                var servicesModel = new StudentEditServicesModel();
                var classStudentModel = studentModel.ClassStudent.FirstOrDefault();
                servicesModel.Name = studentModel.Name;
                servicesModel.StudentCode = studentModel.StudentCode;
                servicesModel.StudentId = studentModel.StudentId;
                servicesModel.Birthday = studentModel.Birthday;
                servicesModel.Gender = studentModel.Gender;
                // classid == 0 -> no classid in db
                servicesModel.OldClassId = (classStudentModel != null) ? classStudentModel.ClassId : 0;
                return servicesModel;
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public async Task UpdateStudent(StudentEditServicesModel servicesModel)
        {
            try
            {
                // get old student model and update information to this model
                var studentModel = Get(servicesModel.StudentId);
                studentModel.Name = servicesModel.Name;
                studentModel.Gender = servicesModel.Gender;
                studentModel.Birthday = servicesModel.Birthday;
                studentModel.StudentCode = servicesModel.StudentCode;
                unitOfWork.StudentRepository.Update(studentModel);

                // classid == 0 -> no classid in db -> no update class-student references 
                var classModel = unitOfWork.ClassRepository.Get(servicesModel.OldClassId);
                if (classModel != null)
                {
                    // Student move to new classes
                    if (servicesModel.NewClassId != servicesModel.OldClassId)
                    {
                        // get old class references
                        var classStudentModel = unitOfWork.ClassStudentRepository.GetClassStudent(servicesModel.OldClassId, servicesModel.StudentId);
                        classStudentModel.ClassId = servicesModel.NewClassId;
                        classStudentModel.Role = (int)Role.MEMBER;
                        // update student information and its references to class                
                        unitOfWork.ClassStudentRepository.Update(classStudentModel);
                    }
                }
                else
                {   // create new class references 
                    var classStudentModel = new ClassStudent();
                    classStudentModel.StudentId = servicesModel.StudentId;
                    classStudentModel.ClassId = servicesModel.NewClassId;
                    classStudentModel.Role = (int)Role.MEMBER;
                    unitOfWork.ClassStudentRepository.Insert(classStudentModel, false);
                }

                int roweffected = await unitOfWork.SaveChange();
                if (roweffected == 0) throw new CustomeException("No effected in database");
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }

        public List<StudentDetailServicesModel> GetStudentListDetail(string text, int classid, string gender, int skip, int take, out int recordsFiltered)
        {
            var baseQuery = unitOfWork.StudentRepository.GetAll();
            var query = unitOfWork.StudentRepository.FilterByTextDetail(baseQuery, text);
            if (classid != 0) query = unitOfWork.StudentRepository.FilterByClass(query, classid);
            if (!string.IsNullOrEmpty(gender))
            {
                bool sex = (!gender.Contains("female", StringComparison.InvariantCultureIgnoreCase));
                query = unitOfWork.StudentRepository.FilterByGender(query, sex);
            }
            recordsFiltered = query.Count();
            if (take != 0) query = query.Skip(skip).Take(take);
            var result = query.Select(res =>
               new StudentDetailServicesModel()
               {
                   StudentId = res.StudentId,
                   Birthday = res.Birthday,
                   Name = res.Name,
                   StudentCode = res.StudentCode,
                   Gender = res.Gender,
                   ClassName = res.ClassStudent.FirstOrDefault().Class.Name,
                   Subjects = string.Join(" ", res.ClassStudent.FirstOrDefault().Class.ClassSubject.Select(cs => cs.Subject.Name).ToArray())
               }
            );
            return result.ToList();
        }
    }
}