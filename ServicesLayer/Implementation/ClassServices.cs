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

namespace ServicesLayer.Implementation
{
    public class ClassServices : IClassServices
    {
        private readonly UnitOfWork unitOfWork;

        public ClassServices(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ClassModel> GetWithIDList(int[] idList) => unitOfWork.ClassRepository.GetWithIDList(idList).ToList();
        public List<ClassModel> GetAll() => unitOfWork.ClassRepository.GetAll().ToList();
        public List<ClassModel> GetAllDetail() => unitOfWork.ClassRepository.GetAllDetail().ToList(); // for greate json file
        public int GetCounting() => unitOfWork.ClassRepository.GetCounting();
        public ClassModel Get(int id) => unitOfWork.ClassRepository.Get(id) ?? throw new CustomeException("Null class object");
        public async Task Delete(int id)
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
        public void AddClass(ClassModel _class)
        {
            try
            {
                unitOfWork.ClassRepository.Insert(_class, true);
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<ClassDetailServicesModel> GetClassListDetail(string text, int skip, int take, int min, int max, params string[] properties)
        {
            try
            {
                var baseQuery = unitOfWork.ClassRepository.GetAllDetail();
                var query = unitOfWork.ClassRepository.FilterByText(baseQuery, text);
                query = unitOfWork.ClassRepository.GetPaging(query, skip, take);

                var result = query.Select(q => new ClassDetailServicesModel()
                {
                    ClassId = q.ClassId,
                    ClassName = q.Name,
                    PersidentId = q.ClassStudent.Where(cs => cs.Role == (int)Role.PRESIDENT).DefaultIfEmpty().First().StudentId,
                    PersidentName = q.ClassStudent.Where(cs => cs.Role == (int)Role.PRESIDENT).DefaultIfEmpty().First().Student.Name??"",
                    SecretaryId = q.ClassStudent.Where(cs => cs.Role == (int)Role.SECRETARY).DefaultIfEmpty().First().StudentId,
                    SecretaryName = q.ClassStudent.Where(cs => cs.Role == (int)Role.SECRETARY).DefaultIfEmpty().First().Student.Name??"",
                    Quantity = q.ClassStudent.Count(),
                    GirlQuantity = q.ClassStudent.Count(cs => cs.Student.Gender == true),
                    BoyQuantity = q.ClassStudent.Count(cs => cs.Student.Gender == false),
                    Subjects = string.Join("|", q.ClassSubject.Select(cs => cs.Subject.Name).ToArray())
                });

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
        public ClassEditServicesModel GetClassEdit(int classId)
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
                model.OldPresidentId = (president.Any()) ? president[0].StudentId : 0;
                model.OldSecretaryId = (secreatary.Any()) ? secreatary[0].StudentId : 0;
                return model;
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }

        private ClassStudent[] ListStudentEffected(ClassEditServicesModel servicesModel)
        {
            // the student has old role and new role
            var listStudentEffected = new List<ClassStudent>();

            //Reset old president and secreatary to member role
            var resetStudentRole = unitOfWork.ClassStudentRepository.Find(cs => cs.StudentId == servicesModel.OldPresidentId || cs.StudentId == servicesModel.OldSecretaryId);
            resetStudentRole.ToList().ForEach(cs => cs.Role = 0);
            listStudentEffected.AddRange(resetStudentRole);
            //  update new role
            var newPresident = unitOfWork.ClassStudentRepository.GetClassStudent(servicesModel.ClassId, servicesModel.NewPresidentId);
            var newSecreatary = unitOfWork.ClassStudentRepository.GetClassStudent(servicesModel.ClassId, servicesModel.NewSecretaryId);
            if (newPresident != null)
            {
                newPresident.Role = (int)Role.PRESIDENT;
                listStudentEffected.Add(newPresident);
            }
            if (newSecreatary != null)
            {
                newSecreatary.Role = (int)Role.SECRETARY;
                listStudentEffected.Add(newSecreatary);
            }
            return listStudentEffected.ToArray();
        }

        public async Task UpdateClass(ClassEditServicesModel servicesModel)
        {
            // one student has only one role and ID must != 0
            if (servicesModel.NewPresidentId == servicesModel.NewSecretaryId && servicesModel.NewPresidentId != 0)
            {
                throw new CustomeException("A class has only one president and one secretary");
            }
            try
            {
                var classModel = Get(servicesModel.ClassId);

                classModel.Name = servicesModel.Name;
                var listStudentEffected = ListStudentEffected(servicesModel);
                if (listStudentEffected.Length > 0)
                {
                    unitOfWork.ClassStudentRepository.UpdateRange(listStudentEffected);
                }
                unitOfWork.ClassRepository.Update(classModel);
                int roweffected = await unitOfWork.SaveChange();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
    }
}
