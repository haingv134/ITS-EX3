using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class ClassStudent
    {
        public int ClassStudentId { get; set; } // PK
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
        public int Role { get; set; }

        [ForeignKey("ClassId")]
        public virtual ClassModel Class { get; set; } // Fk

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } // Fk

        public bool IsAvaiable {get; set; }

    }
}
