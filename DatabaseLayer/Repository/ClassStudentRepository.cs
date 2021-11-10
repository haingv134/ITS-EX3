
using DatabaseLayer.Context;
using DatabaseLayer.Entity;
using DatabaseLayer.Repository.Interface;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;

namespace DatabaseLayer.Repository
{
    public class ClassStudentRepository : GenericRepository<ClassStudent>, IClassStudentRepository
    {
        private readonly DatabaseContext _dbContext;
        // parse dbcontext to base class -> use all method of base class 
        public ClassStudentRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public ClassStudent GetClassStudent(int classId, int studentId) => _dbContext.ClassStudents.Where(cs => cs.ClassId == classId && cs.StudentId == studentId).FirstOrDefault();
        public IQueryable<Student> GetStudentByRole(int classId, int role)
        {
            var listStudent = _dbContext.ClassStudents.Where(cs => cs.ClassId == classId && cs.Role == role);
            return listStudent.Include(cs => cs.Student).Select(cs => cs.Student);
        }
        public void DeleteStudentInClass(int classid)
        {
            _dbContext.ClassStudents.RemoveRange(_dbContext.ClassStudents.Where(cs => cs.ClassId == classid).ToArray());
        }
    }
}
