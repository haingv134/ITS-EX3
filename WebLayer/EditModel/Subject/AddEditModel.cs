using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLayer.EditModel.Subject
{
    public class AddEditModel
    {
        public Guid SubjectId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} phải có chiều dài từ 2 - 20")]
        [Display(Name = "Tên môn học")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} phải có chiều dài từ 2 - 20")]
        [Display(Name = "Mã lớp")]
        public string SubjectCode { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndTime { get; set; }
    }
}
