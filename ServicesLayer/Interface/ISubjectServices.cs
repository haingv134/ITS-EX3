using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using ServicesLayer.ViewModel;

namespace ServicesLayer.Interface
{
    public interface ISubjectServices
    {
        public Subject Get(int id);
        public List<Subject> GetAll();
        public List<Subject> GetAllDetail(); // for greate json file
        public void AddSubject(Subject subject);
        public Task AddClassSubject(ClassAddSubjectServicesModel csModel);
        public Task Delete(int id);
    }
}
