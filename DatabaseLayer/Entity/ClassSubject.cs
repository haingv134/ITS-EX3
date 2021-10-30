using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class ClassSubject
    {
        public int ClassSubjectId { get; set; }
        
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual ClassModel Class { get; set; } // Fk

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } // Fk
    }
}
