using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLayer.EditModel.Student
{
    public class EditEditModel
    {
        public int OldClassId { get; set; }
        public int NewClassId { get; set; }
        public int StudentId { get; set; }
        [Required]
        [Display(Name = "Ten sinh vien")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ban phai nhap ten co do dai tu 2 -> 50")]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
    }
}
