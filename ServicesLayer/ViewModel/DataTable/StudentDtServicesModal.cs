using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.ViewModel;

namespace ServicesLayer.ViewModel.DataTable
{
    public class StudentDtServicesModal
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<StudentDetailServicesModel> data { get; set; }
    }
}
