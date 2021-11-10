using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebLayer.EditModel.Class
{
    public class EditEditModel
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length require from 3 - 20")]
        [Display(Name = "Ten lop")]
        public string Name { get; set; }
        public List<SelectListItem> StudentList {get; set;}
        public List<SelectListItem> SubjectList {get; set;}
        public int[] StudentId { get; set; }
        public int[] SubjectId { get; set; }
        public int OldPresidentId { get; set; }
        public int OldSecretaryId { get; set; }
        public int NewPresidentId { get; set; }
        public int NewSecretaryId { get; set; }
    }
}
