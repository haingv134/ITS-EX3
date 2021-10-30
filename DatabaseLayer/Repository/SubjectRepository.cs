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
    }
}
