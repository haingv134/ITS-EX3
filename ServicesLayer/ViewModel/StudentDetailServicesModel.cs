using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ViewModel
{
    public class StudentDetailServicesModel
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string StudentCode { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string ClassName { get; set; }
        public string Subjects { get; set; }
    }
}
