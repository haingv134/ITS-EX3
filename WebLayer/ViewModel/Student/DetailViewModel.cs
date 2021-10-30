using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLayer.ViewModel.Student
{
    public class DetailViewModel
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string ClassName { get; set; }
        public string[] Subjects { get; set; }
    }
}
