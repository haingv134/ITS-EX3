using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;

namespace DatabaseLayer.Repository.Interface
{

    // dont need implement from IGenericRepository
    // IGenericRepository just need implemented by GenericRepository
    public interface IClassRepository
    {
        public IQueryable<ClassModel> FilterByText(IQueryable<ClassModel> source, string text);        
    }
}
