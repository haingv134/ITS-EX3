using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class Subject
    {
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvaiable {get; set; }
        public virtual IEnumerable<ClassSubject> ClassSubject { get; set; }
    }
}
