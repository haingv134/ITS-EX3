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
    public class ClassRepository : GenericRepository<ClassModel>, IClassRepository
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
        public IQueryable<ClassModel> FilterByText(IQueryable<ClassModel> source, string text)
        {
            return source.Where(cl => cl.ClassStudent.Where(cs => cs.Student.Name.ToLower().Contains(text)).Any()
                                  && cl.ClassStudent.Where(cs => cs.Role == 1 || cs.Role == 2).Any() || cl.Name.ToLower().Contains(text));
        }
    }
}

