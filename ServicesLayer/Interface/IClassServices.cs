using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using ServicesLayer.ViewModel;

namespace ServicesLayer.Interface
{
    public interface IClassServices
    {
        public List<ClassModel> GetAll();
        public List<ClassModel> GetAllDetail(); // for generate json file
        public int GetCounting();
        public ClassModel Get(int id);
        public Task Delete(int id);
        public void AddClass(ClassModel _class);
        public List<ClassModel> FilterByText(IQueryable<ClassModel> source, string text, params string[] propertiesName);
        public List<ClassDetailServicesModel> GetClassListDetail();
        public List<ClassDetailServicesModel> GetClassListDetail(int skip, int take);
        public List<ClassDetailServicesModel> GetClassListDetail(List<ClassModel> list, int skip, int take);
        public List<ClassDetailServicesModel> GetClassListDetailWithRangeCondition(List<ClassModel> list, int min, int max, int skip, int take, params string[] propertiesName);
        public List<ClassDetailServicesModel> GetClassMaxBoy();
        public ClassEditServicesModel GetClassEdit(int classId);
        public Task UpdateClass(ClassEditServicesModel servicesModel);
        public List<ClassModel> FilterByText(string text);        
    }
}
