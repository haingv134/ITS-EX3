using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class ClassModel
    {
        public Guid ClassId { get; set; }    
        public string Name { get; set; }
        public int MaxStudent {get; set;}
        public bool IsAvaiable {get; set; }
        // Collection Navigation
        public virtual IEnumerable<ClassStudent> ClassStudent { get; set; }
        // Collection Navigation
        public virtual IEnumerable<ClassSubject> ClassSubject { get; set; }
    }
}
