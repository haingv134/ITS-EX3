using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
namespace WebLayer.EditModel.Student
{
    public class AddEditModel
    {
        //int classId, [Bind("Name", "Birthday", "Gender")] StudentModel student, int role
        public int ClassId { get; set; }
        [Required]
        [Display(Name = "Ten sinh vien")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ban phai nhap ten co do dai tu 2 -> 50")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Mã sinh vien")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ban phai nhap ma co do dai tu 2 -> 50")]
        public string StudentCode { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
    }
}
