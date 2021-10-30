using System.ComponentModel.DataAnnotations;

namespace WebLayer.EditModel.Class
{
    public class EditEditModel
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length require from 3 - 20")]
        [Display(Name = "Ten lop")]
        public string Name { get; set; }

        public int OldPresidentId { get; set; }
        public int OldSecretaryId { get; set; }
        public int NewPresidentId { get; set; }
        public int NewSecretaryId { get; set; }
    }
}
