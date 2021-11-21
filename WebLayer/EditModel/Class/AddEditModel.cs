using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
namespace WebLayer.EditModel.Class
{
    public class AddEditModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length require from 3 - 20")]
        [Display(Name = "Ten lop")]
        public string Name { get; set; }
        public Guid[] StudentId { get; set; }
        public Guid[] SubjectId { get; set; }
        public Guid PersidentId {get; set;}
        public Guid SecretaryId {get; set;}
    }
}
