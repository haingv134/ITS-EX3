using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebLayer.EditModel.Class
{
    public class EditEditModel
    {
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length require from 3 - 20")]
        [Display(Name = "Ten lop")]
        public string Name { get; set; }
        public List<SelectListItem> StudentList {get; set;}
        public List<SelectListItem> SubjectList {get; set;}
        public Guid[] StudentId { get; set; }
        public Guid[] SubjectId { get; set; }
        public Guid OldPresidentId { get; set; }
        public Guid OldSecretaryId { get; set; }
        public Guid NewPresidentId { get; set; }
        public Guid NewSecretaryId { get; set; }
    }
}
