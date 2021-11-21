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
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly DatabaseContext _dbContext;
        public SubjectRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Subject> GetAllDetails() => _dbContext.Subjects.Include(subject => subject.ClassSubject);
        public IQueryable<Subject> GetSubjectInClass(Guid classid) => _dbContext.Subjects.Include(s => s.ClassSubject)
                                                                                            .Where(s => s.ClassSubject.Any());
        public IQueryable<Subject> GetSubjectByStudent(Guid studentId)
        {
            var classid = _dbContext.ClassStudents.Where(cs => cs.StudentId == studentId).SingleOrDefault();

            if (classid != null)
            {

                return _dbContext.ClassSubjects.Include(cs => cs.Subject).Where(cs => cs.ClassId == classid.ClassId)
                                                .Select(cs => cs.Subject);
            }
            else return null;
        }
    }
}
