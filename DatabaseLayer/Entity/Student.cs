using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentCode {get; set;}
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public virtual IEnumerable<ClassStudent> ClassStudent { get; set; }      
    }
}
