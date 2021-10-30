using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<ClassSubject> ClassSubject { get; set; }
    }
}
