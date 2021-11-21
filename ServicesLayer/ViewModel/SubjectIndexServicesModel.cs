using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ViewModel
{
    public class SubjectIndexServicesModel
    {
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvaiable {get; set; }
    }
}
