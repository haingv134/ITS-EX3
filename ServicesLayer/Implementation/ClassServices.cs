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

        public ClassDetailServicesModel GetClassDetail(int classId)
        {
            try
            {
                var classModel = unitOfWork.ClassRepository.GetClassDetail(classId) ?? throw new CustomeException("Null class object"); ;
                var classDetailServicesModel = new ClassDetailServicesModel();
                var president = unitOfWork.ClassStudentRepository.GetStudentByRole(classId, (int)Role.PRESIDENT).ToList();
                var secreatary = unitOfWork.ClassStudentRepository.GetStudentByRole(classId, (int)Role.SECRETARY).ToList();
                // assign value for services modal
                classDetailServicesModel.ClassId = classModel.ClassId;
                classDetailServicesModel.ClassName = classModel.Name;
                classDetailServicesModel.PersidentId = (president.Any()) ? president[0].StudentId : 0;
                classDetailServicesModel.PersidentName = (president.Any()) ? president[0].Name : string.Empty;
                classDetailServicesModel.SecretaryId = (secreatary.Any()) ? secreatary[0].StudentId : 0;
                classDetailServicesModel.SecretaryName = (secreatary.Any()) ? secreatary[0].Name : string.Empty;
                classDetailServicesModel.Quantity = unitOfWork.StudentRepository.Count(classId);
                classDetailServicesModel.BoyQuantity = unitOfWork.StudentRepository.CountGender(classId, true);
                classDetailServicesModel.GirlQuantity = unitOfWork.StudentRepository.CountGender(classId, false);
                classDetailServicesModel.Subjects = string.Join("|", classModel.ClassSubject.Select(cs => cs.Subject.Name).ToArray());
                return classDetailServicesModel;
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }

        public List<ClassDetailServicesModel> GetClassListDetail()
        {
            try
            {
                return unitOfWork.ClassRepository.GetAll().ToList().Select(_class => GetClassDetail(_class.ClassId)).ToList();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<ClassDetailServicesModel> GetClassListDetail(int skip, int take)
        {
            try
            {
                return unitOfWork.ClassRepository.GetPaging(skip, take).Select(_class => GetClassDetail(_class.ClassId)).ToList();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<ClassDetailServicesModel> GetClassListDetail(List<ClassModel> list, int skip, int take)
        {
            try
            {
                return list.Skip(skip).Take(take).Select(_class => GetClassDetail(_class.ClassId)).ToList();
            }
            catch (CustomeException e)
            {
                throw new CustomeException(e.Messages);
            }
        }
        public List<ClassDetailServicesModel> GetClassListDetailWithRangeCondition(List<ClassModel> list, int min, int max, int skip, int take, params string[] propertiesName)
        {
            try
            {
                return list.Select(_class => GetClassDetail(_class.ClassId)).AsQueryable().FilterWithRange(min, max, propertiesName).Skip(skip).Take(take).ToList();
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
                return unitOfWork.ClassRepository.GetClassMaxBoy().Select(_class => GetClassDetail(_class.ClassId)).ToList();
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
                var classModel = unitOfWork.ClassRepository.GetClassDetail(classId)??throw new CustomeException("Class null object");
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
        public List<ClassModel> FilterByText(IQueryable<ClassModel> source, string text, params string[] propertiesName)
        {
            return unitOfWork.ClassRepository.FilterByText(source, text, propertiesName).ToList();
        }
        public List<ClassModel> FilterByText(string text) => unitOfWork.ClassRepository.FilterByText(text).ToList();

    }
}
