using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;

namespace ServicesLayer.ViewModel
{
    public class StudentEditServicesModel
    {
        public Guid OldClassId { get; set; }
        public Guid NewClassId { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string StudentCode { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
    }
}
