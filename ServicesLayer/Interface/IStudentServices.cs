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
        public Student Get(int id);
        public Task Delete(int id);
        public List<Student> GetAll();
        public int GetCounting();
        public List<Student> GetStudentListByClass(int classId);
        public List<Student> GetAvaibleStudentWithClass(int classid);
        public List<StudentOldServicesModel> GetYoungestStudent();
        public List<StudentOldServicesModel> GetOldestStudent();
        public List<Student> GetAvaiableStudent();
        public StudentEditServicesModel GetStudentEdit(int studentId);
        public Task UpdateStudent(StudentEditServicesModel servicesModel);
        public List<StudentDetailServicesModel> GetStudentListDetail(string text, int classid, string gender, int skip, int take, out int recordsFiltered);
    }
}
