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
        public int Quantity { get; set; }
        public int BoyQuantity { get; set; }
        public int GirlQuantity { get; set; }
        public int MaxStudent { get; set; }
    }
}
