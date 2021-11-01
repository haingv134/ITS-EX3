using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLayer.EditModel.Subject
{
    public class AddEditModel
    {
        public int SubjectId {get; set;}
        [Required(ErrorMessage = "This field is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Length require from 2 - 20")]
        [Display(Name = "Ten Mon hoc")]
        public string Name { get; set; }
    }
}
