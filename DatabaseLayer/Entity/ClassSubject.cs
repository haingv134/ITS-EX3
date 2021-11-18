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

        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public bool IsAvaiable { get; set; }

        [ForeignKey("ClassId")]
        public virtual ClassModel Class { get; set; } // Fk

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } // Fk
    }
}
