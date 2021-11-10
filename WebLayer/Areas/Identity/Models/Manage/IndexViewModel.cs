using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Identity.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Display(Name = "Tên đầy đủ")]
        [StringLength(50, ErrorMessage = "{0} phải có chiều dài từ {2} tới {1}", MinimumLength = 3)]
        public string FullName { get; set; }
        public string Website { get; set; }
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ngày Sinh")]
        public DateTime? Birthday { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Thông tin chi tiết")]
        public string About { get; set; }
    }
}
