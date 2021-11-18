using DatabaseLayer.Entity;
using DatabaseLayer.Enum;
using DatabaseLayer.ExceptionHandling;
using DatabaseLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using ServicesLayer.Interface;
using ServicesLayer.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicesLayer.ExtensionMethod;
using System;

namespace ServicesLayer.Implementation
{
    public class ClassServices : IClassServices
    {
        private readonly UnitOfWork unitOfWork;

        public ClassServices(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ClassModel> GetWithIDList(Guid[] idList) => unitOfWork.ClassRepository.GetWithIDList(idList).ToList();
        public List<ClassModel> GetAll() => unitOfWork.ClassRepository.GetAll().ToList();
        public List<ClassModel> GetAllDetail() => unitOfWork.ClassRepository.GetAllDetail().ToList(); // for greate json file
        public int GetCounting() => unitOfWork.ClassRepository.GetCounting();
        public ClassModel Get(Guid id) => unitOfWork.ClassRepository.Get(id) ?? throw new CustomeException("Null class object");
        public async Task Delete(Guid id)
        {
            try
            {
                var classModal = Get(id);
                unitOfWork.ClassRepository.Remove(classModal);
                int roweffected = await unitOfWork.SaveChange();
                if (roweffected == 0) throw new CustomeException("No record effected in database");
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public async Task DeleteRange(Guid[] id)
        {
            try
            {
                var classListModal = GetWithIDList(id);
                unitOfWork.ClassRepository.RemoveRange(classListModal.ToArray());
                int roweffected = await unitOfWork.SaveChange();
                if (roweffected == 0) throw new CustomeException("No record effected in database");
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public async Task AddClass(ClassAddServicesModel source)
        {
            try
            {
                if (source.SecretaryId == source.PersidentId && source.SecretaryId != Guid.Empty)
                    throw new CustomeException("A class have only one persident and one secretary");
                var classModel = new ClassModel() { Name = source.Name };
                Guid classId = unitOfWork.ClassRepository.Insert(classModel, true).ClassId;
                if (source.StudentId.Length > 0)
                {
                    var classStudent = new List<ClassStudent>();
                    for (int index = 0; index < source.StudentId.Length; index++)
                    {
                        int role = 0;
                        Guid studentId = source.StudentId[index];
                        if (source.PersidentId == studentId) role = (int)Role.PRESIDENT;
                        if (source.SecretaryId == studentId) role = (int)Role.SECRETARY;
                        classStudent.Add(new ClassStudent()
                        {
                            ClassId = classId,
                            StudentId = studentId,
                            Role = role
                        });
                        unitOfWork.ClassStudentRepository.InsertRange(classStudent.ToArray());
                    }
                }
                if (source.SubjectId.Length > 0)
                {
                    var classSubject = new List<ClassSubject>();
                    for (int index = 0; index < source.SubjectId.Length; index++)
                    {
                        classSubject.Add(new ClassSubject()
                        {
                            ClassId = classId,
                            SubjectId = source.SubjectId[index]
                        });
                    }
                    unitOfWork.ClassSubjectRepository.InsertRange(classSubject.ToArray());
                }
                await unitOfWork.SaveChange();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<ClassDetailServicesModel> GetClassListDetail(string text, int skip, int take, int min, int max, string properties, out int recordFilterd)
        {
            try
            {
                var baseQuery = unitOfWork.ClassRepository.GetAllDetail();
                var query = unitOfWork.ClassRepository.FilterByText(baseQuery, text);
                var result = query.Select(q => new ClassDetailServicesModel()
                {
                    ClassId = q.ClassId,
                    ClassName = q.Name,
                    PersidentId = q.ClassStudent.Where(cs => cs.Role == (int)Role.PRESIDENT).DefaultIfEmpty().First().StudentId,
                    PersidentName = q.ClassStudent.Where(cs => cs.Role == (int)Role.PRESIDENT).DefaultIfEmpty().First().Student.Name ?? "",
                    SecretaryId = q.ClassStudent.Where(cs => cs.Role == (int)Role.SECRETARY).DefaultIfEmpty().First().StudentId,
                    SecretaryName = q.ClassStudent.Where(cs => cs.Role == (int)Role.SECRETARY).DefaultIfEmpty().First().Student.Name ?? "",
                    Quantity = q.ClassStudent.Count(),
                    GirlQuantity = q.ClassStudent.Count(cs => cs.Student.Gender == true),
                    BoyQuantity = q.ClassStudent.Count(cs => cs.Student.Gender == false),
                    Subjects = string.Join("|", q.ClassSubject.Select(cs => cs.Subject.Name).ToArray())
                });
                if (!string.IsNullOrEmpty(properties)) result = result.FilterWithRange(min, max, properties);
                recordFilterd = result.Count();
                if (take != 0) result = result.Skip(skip).Take(take);
                return result.ToList();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<ClassDetailServicesModel> GetClassMaxBoy()
        {
            try
            {
                return null;
                //return unitOfWork.ClassRepository.GetClassMaxBoy().Select(_class => GetClassDetail(_class.ClassId)).ToList();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }

        // get information for editing class -> transfer to view
        public ClassEditServicesModel GetClassEdit(Guid classId)
        {
            var model = new ClassEditServicesModel();
            try
            {
                var classModel = unitOfWork.ClassRepository.Get(classId) ?? throw new CustomeException("Class null object");
                var president = unitOfWork.ClassStudentRepository.GetStudentByRole(classId, (int)Role.PRESIDENT).ToList();
                var secreatary = unitOfWork.ClassStudentRepository.GetStudentByRole(classId, (int)Role.SECRETARY).ToList();
                // getting requre information to view
                model.ClassId = classModel.ClassId;
                model.Name = classModel.Name;
                model.OldPresidentId = (president.Any()) ? president[0].StudentId : Guid.Empty;
                model.OldSecretaryId = (secreatary.Any()) ? secreatary[0].StudentId : Guid.Empty;

                return model;
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }

        public async Task UpdateClass(ClassEditServicesModel source)
        {
            // one student has only one role and ID must != 0
            if (source.NewPresidentId == source.NewSecretaryId && source.NewPresidentId != Guid.Empty)
            {
                throw new CustomeException("A class has only one president and one secretary");
            }
            try
            {
                var classModel = Get(source.ClassId);
                classModel.Name = source.Name;                
                unitOfWork.ClassRepository.Update(classModel);

                if (source.StudentId.Length > 0)
                {
                    var classStudent = new List<ClassStudent>();
                    for (int index = 0; index < source.StudentId.Length; index++)
                    {
                        int role = 0;
                        Guid studentId = source.StudentId[index];
                        if (source.NewPresidentId == studentId) role = (int)Role.PRESIDENT;
                        if (source.NewSecretaryId == studentId) role = (int)Role.SECRETARY;
                        classStudent.Add(new ClassStudent()
                        {
                            ClassId = source.ClassId,
                            StudentId = studentId,
                            Role = role
                        });
                        unitOfWork.ClassStudentRepository.DeleteStudentInClass(source.ClassId);
                        unitOfWork.ClassStudentRepository.InsertRange(classStudent.ToArray());
                    }
                }
                if (source.SubjectId.Length > 0)
                {
                    var classSubject = new List<ClassSubject>();
                    for (int index = 0; index < source.SubjectId.Length; index++)
                    {
                        classSubject.Add(new ClassSubject()
                        {
                            ClassId = source.ClassId,
                            SubjectId = source.SubjectId[index]
                        });
                    }
                    unitOfWork.ClassSubjectRepository.DeleteSubjectInClass(source.ClassId);
                    unitOfWork.ClassSubjectRepository.InsertRange(classSubject.ToArray());
                }

                int roweffected = await unitOfWork.SaveChange();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
    }
}
