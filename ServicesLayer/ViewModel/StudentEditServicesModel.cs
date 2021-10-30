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
        public int OldClassId { get; set; }
        public int NewClassId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
    }
}
