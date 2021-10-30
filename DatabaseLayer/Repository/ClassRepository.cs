using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using DatabaseLayer.Repository.Interface;
using DatabaseLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatabaseLayer.Repository
{
    public class ClassRepository :  GenericRepository<ClassModel>, IClassRepository
    {
        private readonly DatabaseContext _dbContext;
        // parse dbcontext to base class -> use all method of base class 
        public ClassRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;         
        }   
        public IQueryable<ClassModel> GetAllDetail()
        {
            return _dbContext.Classes.Include(_class => _class.ClassStudent)
                                            .ThenInclude(cs => cs.Student)
                                        .Include(_class => _class.ClassSubject)
                                            .ThenInclude(cs => cs.Subject);
        }
        public ClassModel GetClassDetail(int classId)
        {
            return _dbContext.Classes.Include(_class => _class.ClassStudent)
                                   .ThenInclude(classStudent => classStudent.Student)
                               .Include(_class => _class.ClassSubject)
                                   .ThenInclude(classSubject => classSubject.Subject)
                                .Where(cl => cl.ClassId == classId)
                                .FirstOrDefault();
        }
        public IQueryable<ClassModel> GetClassMaxBoy() //.AsSplitQuery() // or asSingleQuery (by default)
        {
            var classWithCount = _dbContext.ClassStudents.Include(cs => cs.Student)
                                    .Where(cs => cs.Student.Gender)
                                    .GroupBy(cs => cs.ClassId)
                                    .Select(cs => new
                                    {
                                        ClassId = cs.Key,
                                        Count = cs.Count()
                                    });                                    

            var classList =  classWithCount.Where(cl => cl.Count == classWithCount.Max(clc => clc.Count))
                            .Select(cl => cl.ClassId);
            return _dbContext.Classes.Where(cl => classList.Contains(cl.ClassId));
        }
        public IQueryable<ClassModel> FilterByText(string text)
        {
            return _dbContext.Classes.Include(cl => cl.ClassStudent)
                                 .Include(cl => cl.ClassSubject)
                                 .Where(cl => cl.ClassStudent.Where(cs => cs.Student.Name.ToLower().Contains(text)).Any() && cl.ClassStudent.Where(cs => cs.Role == 1 || cs.Role == 2).Any() || cl.Name.ToLower().Contains(text));
        }
    }
}
