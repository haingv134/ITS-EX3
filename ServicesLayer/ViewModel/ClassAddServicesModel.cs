using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;

namespace ServicesLayer.ViewModel
{
    public class ClassAddServicesModel
    {
        public string Name { get; set; }
        public Guid[] StudentId { get; set; }
        public Guid[] SubjectId { get; set; }
        public Guid PersidentId {get; set;}
        public Guid SecretaryId {get; set;}
    }
}