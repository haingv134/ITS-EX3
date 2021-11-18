using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;

namespace ServicesLayer.ViewModel
{
    public class ClassDetailServicesModel
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public Guid SecretaryId { get; set; }
        public string SecretaryName { get; set; }
        public Guid PersidentId { get; set; }
        public string PersidentName { get; set; }
        public int Quantity { get; set; }
        public int BoyQuantity { get; set; }
        public int GirlQuantity { get; set; }
        public string Subjects { get; set; }
    }
}
