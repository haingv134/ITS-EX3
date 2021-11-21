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
        public Subject Get(Guid id);
        public int GetCounting();
        public List<Subject> GetAll();
        public List<SubjectIndexServicesModel> GetAll(int skip, int take);
        public List<Subject> GetSubjectListByClass(Guid classid);
        public List<Subject> GetSubjectListByStudent(Guid studentId);
        
        public void AddSubject(Subject subject);
        public Task AddClassSubject(ClassAddSubjectServicesModel csModel);
        public Task Delete(Guid id);
        public Task Update(Subject subjectModel);
    }
}
