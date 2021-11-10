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
        public int[] StudentId { get; set; }
        public int[] SubjectId { get; set; }
        public int PersidentId {get; set;}
        public int SecretaryId {get; set;}
    }
}