using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Entity.Identity
{
    public class AppUser : IdentityUser
    {        
        [StringLength(50, ErrorMessage = "{0} phải có chiều dài từ {2} tới {1}", MinimumLength = 3)]        
        public string FullName { get; set; }        
        [PersonalData]
        public string Address { get; set; }        
        [PersonalData]
        public DateTime? Birthday { get; set; }        
        [PersonalData]
        public Byte[] Image { get; set; }
        public string About {get; set;}
        public string Website {get; set;}
    }
}
