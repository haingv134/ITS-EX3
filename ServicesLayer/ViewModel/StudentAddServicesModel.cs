using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;

namespace ServicesLayer.ViewModel
{
    public class StudentAddServicesModel
    {
        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public string StudentCode { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }

        public Student GetStudent() => new Student() { Birthday = Birthday, Gender = Gender, Name = Name, StudentCode = StudentCode };
        public ClassStudent GetClassStudent() => new ClassStudent() { ClassId = ClassId, Role = 0};
    }
}
