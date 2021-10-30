using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
namespace DatabaseLayer.Repository.Interface
{
    public interface IStudentRepository
    {
        public IQueryable<Student> GetAllDetails();
        public IQueryable<Student> GetYoungestStudent();
        public IQueryable<Student> GetOldestStudent();
        public IQueryable<Student> GetStudentListbyClass(int classId);
        public Student GetStudentDetail(int studentId);
        public IQueryable<Student> FilterByText(string text);
        public IQueryable<Student> FilterByTextDetail(string text);
        public IQueryable<Student> FilterByGender(bool gender);
    }
}
