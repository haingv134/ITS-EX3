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
        public List<Student> GetAllDetail(); // for greate json file
        public int GetCounting();
        public List<Student> GetStudentListByClass(int classId);
        public List<StudentOldServicesModel> GetYoungestStudent();
        public List<StudentOldServicesModel> GetOldestStudent();
        public StudentEditServicesModel GetStudentEdit(int studentId);
        public Task UpdateStudent(StudentEditServicesModel servicesModel);
        public List<Student> FilterByText(string text);
        public List<Student> FilterByGender(bool gender);
        public List<Student> FilterByTextDetail(string text);
        public List<StudentDetailServicesModel> GetStudentListDetail();
        public List<StudentDetailServicesModel> GetStudentListDetail(List<Student> list, int skip, int take);

    }
}
