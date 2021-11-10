using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.Interface;
using DatabaseLayer.UnitOfWork;
using DatabaseLayer.Entity;
using Microsoft.Extensions.Logging;
using ServicesLayer.ViewModel;
using DatabaseLayer.ExceptionHandling;

namespace ServicesLayer.Implementation
{
    public class SubjectServices : ISubjectServices
    {
        private readonly UnitOfWork unitOfWork;
        public SubjectServices(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public Subject Get(int id) => unitOfWork.SubjectRepository.Get(id) ?? throw new CustomeException("Subject Null Object");
        public int GetCounting() => unitOfWork.SubjectRepository.GetCounting();
        public List<Subject> GetAll() => unitOfWork.SubjectRepository.GetAll().ToList();
        public List<Subject> GetAll(int skip, int take) => unitOfWork.SubjectRepository.GetAll().Skip(skip).Take(take).ToList();
        public List<Subject> GetAllDetail() => unitOfWork.SubjectRepository.GetAllDetails().ToList();
        
        public List<Subject> GetSubjectListByClass(int classid) => unitOfWork.SubjectRepository.GetSubjectInClass(classid).ToList();
        public void AddSubject(Subject subject)
        {
            try
            {
                unitOfWork.SubjectRepository.Insert(subject, true);
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public async Task AddClassSubject(ClassAddSubjectServicesModel csModel)
        {
            try
            {
                foreach (var classId in csModel.ClassId)
                {
                    var listClassSubject = unitOfWork.ClassSubjectRepository.GetSubjectInClass(classId).ToArray();
                    // remove avaiable subject added to class before all
                    unitOfWork.ClassSubjectRepository.RemoveRange(listClassSubject);
                    var subjectAdded = csModel.SubjectId.Select(sid => new ClassSubject()
                    {
                        ClassId = classId,
                        SubjectId = sid
                    }).ToArray();
                    // add/update new subject to class
                    unitOfWork.ClassSubjectRepository.InsertRange(subjectAdded);
                }
                int roweffected = await unitOfWork.SaveChange();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                var subjectModel = Get(id);
                unitOfWork.SubjectRepository.Remove(subjectModel);
                int roweffected = await unitOfWork.SaveChange();
                if (roweffected == 0) throw new CustomeException("No record effected in database");
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public async Task Update(Subject subjectModel)
        {
            try
            {
                var model = Get(subjectModel.SubjectId);
                model.Name = subjectModel.Name;
                unitOfWork.SubjectRepository.Update(model);
                int roweffected = await unitOfWork.SaveChange();
                if (roweffected == 0) throw new CustomeException("No record effected in database");
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
    }
}
