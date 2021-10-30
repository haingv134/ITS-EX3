using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicesLayer.ViewModel;

namespace ServicesLayer.ViewModel.DataTable
{
    public class ClassDtServicesModal
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<ClassDetailServicesModel> data { get; set; }
    }
}