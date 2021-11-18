using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using ServicesLayer.ViewModel;
namespace ServicesLayer.Interface
{
    public interface IStudentServices
    {
        public void AddStudent(StudentAddServicesModel studentServicesModel);
        public Student Get(Guid id);
        public Task Delete(Guid id);
        public List<Student> GetAll();
        public int GetCounting();
        public List<Student> GetStudentListByClass(Guid classId);
        public List<Student> GetAvaibleStudentWithClass(Guid classid);
        public List<StudentOldServicesModel> GetYoungestStudent();
        public List<StudentOldServicesModel> GetOldestStudent();
        public List<Student> GetAvaiableStudent();
        public StudentEditServicesModel GetStudentEdit(Guid studentId);
        public Task UpdateStudent(StudentEditServicesModel servicesModel);
        public List<StudentDetailServicesModel> GetStudentListDetail(string text, Guid classid, string gender, int skip, int take, out int recordsFiltered);
    }
}
