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
        public IQueryable<Student> GetAllDetails() => _dbContext.Students.Include(student => student.ClassStudent);
        public IQueryable<Student> GetYoungestStudent()
        {
            return _dbContext.Students.Where(student => student.Birthday == _dbContext.Students.Max(student => student.Birthday));
        }
        public IQueryable<Student> GetOldestStudent()
        {
            return _dbContext.Students.Where(student => student.Birthday == _dbContext.Students.Min(student => student.Birthday));
        }
        public IQueryable<Student> GetStudentListbyClass(int classId)
        {
            return _dbContext.ClassStudents.Include(cs => cs.Student)
                                            .Where(cs => cs.ClassId == classId)
                                            .Select(cs => cs.Student);
        }
        public Student GetStudentDetail(int studentId)
        {
            return _dbContext.Students.Include(student => student.ClassStudent)
                                            .ThenInclude(cs => cs.Class)
                                                .ThenInclude(cl => cl.ClassSubject)
                                                    .ThenInclude(cs => cs.Subject)
                                        .Where(student => student.StudentId == studentId)
                                        .SingleOrDefault();
        }
        public IQueryable<Student> FilterByText(string text)
        {
            text = text.ToLower();
            return _dbContext.Students.Where(student => student.Name.ToLower().Contains(text) || student.StudentId.ToString().Contains(text));
        }
        public IQueryable<Student> FilterByTextDetail(string text)
        {
            return _dbContext.Students.Include(st => st.ClassStudent)
                                     .ThenInclude(cs => cs.Class)
                                         .ThenInclude(cl => cl.ClassSubject)
                                             .ThenInclude(cs => cs.Subject)
                                 .Where(st => st.Name.ToLower().Contains(text)
                                         || st.StudentId.ToString().Contains(text)
                                         || st.ClassStudent.Any(cs => cs.Class.Name.ToLower().Contains(text))
                                         || st.ClassStudent.Any(cs => cs.Class.ClassSubject.Any(cs => cs.Subject.Name.ToLower().Contains(text)))
                                 );
        }
        public int CountGender(int classId, bool gender)
        {
            return _dbContext.ClassStudents.Include(cs => cs.Student)
                                            .Where(cs => cs.ClassId == classId)
                                            .Count(cs => cs.Student.Gender == gender);
        }
        public int Count(int classId) => _dbContext.ClassStudents.Count(cs => cs.ClassId == classId);
        public IQueryable<Student> FilterByGender(bool gender) => Find(st => st.Gender == gender);
        
    }
}
