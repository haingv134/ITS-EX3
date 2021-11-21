using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ViewModel
{
    public class StudentDetailServicesModel
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string StudentCode { get; set; }
        public string Birthday { get; set; }
        public bool Gender { get; set; }
        public int YearOfEnroll { get; set; }
        public string ExtraInfor { get; set; }
    }
}
