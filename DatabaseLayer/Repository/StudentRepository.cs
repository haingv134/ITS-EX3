using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using DatabaseLayer.Context;
using DatabaseLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayer.Repository
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {

        private readonly DatabaseContext _dbContext;
        public StudentRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Student GetStudentEdit(Guid studentId)
        {
            return _dbContext.Students.Include(st => st.ClassStudent)
                                    .Where(st => st.StudentId == studentId)
                                    .FirstOrDefault();
        }        
        public IQueryable<Student> GetAllDetails()
        {
            return _dbContext.Students.Include(s => s.ClassStudent)
                                        .ThenInclude(cs => cs.Class)
                                            .ThenInclude(c => c.ClassSubject)
                                                .ThenInclude(sj => sj.Subject);
        }

        public IQueryable<Student> FilterByTextDetail(IQueryable<Student> source, string text)
        {
            return source.Where(st => st.Name.ToLower().Contains(text)
                                || st.StudentCode.ToLower().Contains(text)
                                || st.ClassStudent.Any(cs => cs.Class.Name.ToLower().Contains(text))
                                || st.ClassStudent.Any(cs => cs.Class.ClassSubject.Any(cs => cs.Subject.Name.ToLower().Contains(text))));
        }
        public IQueryable<Student> FilterByGender(IQueryable<Student> source, bool gender) => source.Where(st => st.Gender == gender);
        public IQueryable<Student> FilterByClass(IQueryable<Student> source, Guid classId) => source.Where(st => st.ClassStudent.Any(st => st.ClassId == classId));
        public IQueryable<Student> GetYoungestStudent()
        {
            return _dbContext.Students.Where(student => student.Birthday == _dbContext.Students.Max(student => student.Birthday));
        }
        public IQueryable<Student> GetOldestStudent()
        {
            return _dbContext.Students.Where(student => student.Birthday == _dbContext.Students.Min(student => student.Birthday));
        }
        public IQueryable<Student> GetStudentListbyClass(Guid classId)
        {
            return _dbContext.ClassStudents.Include(cs => cs.Student)
                                            .Where(cs => cs.ClassId == classId)
                                            .Select(cs => cs.Student);
        }
        public IQueryable<Student> GetAvaibleStudent()
        {
            return _dbContext.Students.Include(cs => cs.ClassStudent)
                                        .Where(cs => !cs.ClassStudent.Any());
        }
        // get avaiable student that include a class and except student have already being in class
        public IQueryable<Student> GetAvaibleStudentWithClass(Guid classid)
        {
            return _dbContext.Students.Include(cs => cs.ClassStudent)
                                        .Where(cs => !cs.ClassStudent.Any() || cs.ClassStudent.Any(cs => cs.ClassId == classid));
        }
        public int Count(Guid classId) => _dbContext.ClassStudents.Count(cs => cs.ClassId == classId);

    }
}
