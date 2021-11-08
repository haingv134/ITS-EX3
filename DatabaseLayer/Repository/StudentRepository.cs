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
        public Student GetStudentEdit(int studentId)
        {
            return _dbContext.Students.Include(st => st.ClassStudent)
                                    .Where(st => st.StudentId == studentId)
                                    .FirstOrDefault();
        }
        public IQueryable<Student> GetAllDetails()
        {
            return _dbContext.Students.Include(st => st.ClassStudent)
                                    .ThenInclude(cs => cs.Class)
                                        .ThenInclude(cl => cl.ClassSubject)
                                            .ThenInclude(cs => cs.Subject);
        }
        public IQueryable<Student> FilterByTextDetail(IQueryable<Student> source, string text)
        {
            return source.Where(st => st.Name.ToLower().Contains(text)
                                || st.StudentCode.ToLower().Contains(text)
                                || st.ClassStudent.Any(cs => cs.Class.Name.ToLower().Contains(text))
                                || st.ClassStudent.Any(cs => cs.Class.ClassSubject.Any(cs => cs.Subject.Name.ToLower().Contains(text))));
        }
        public IQueryable<Student> FilterByGender(IQueryable<Student> source, bool gender) => source.Where(st => st.Gender == gender);
        public IQueryable<Student> FilterByClass(IQueryable<Student> source, int classId) => source.Where(st => st.ClassStudent.Any(st => st.ClassId == classId));
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
        public int Count(int classId) => _dbContext.ClassStudents.Count(cs => cs.ClassId == classId);

    }
}
