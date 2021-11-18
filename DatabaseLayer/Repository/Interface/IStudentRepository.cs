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
        public IQueryable<Student> FilterByTextDetail(IQueryable<Student> source, string text);
        public IQueryable<Student> FilterByGender(IQueryable<Student> source, bool gender);
        public IQueryable<Student> FilterByClass(IQueryable<Student> source, Guid classId);
        public IQueryable<Student> GetYoungestStudent();
        public IQueryable<Student> GetOldestStudent();
        public IQueryable<Student> GetStudentListbyClass(Guid classId);
        public IQueryable<Student> GetAvaibleStudent();
    }
}
