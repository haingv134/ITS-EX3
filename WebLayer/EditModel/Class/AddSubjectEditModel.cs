using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebLayer.EditModel.Class
{
    public class AddSubjectEditModel
    {
        public Guid[] ClassId { get; set; }
        public List<SelectListItem> ClassList {get; set;}
        public Guid[] SubjectId { get; set; }
    }
}
