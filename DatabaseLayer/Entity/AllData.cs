using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity
{
    public class AllData
    {
        public List<ClassModel> Classes { get; set; }
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
