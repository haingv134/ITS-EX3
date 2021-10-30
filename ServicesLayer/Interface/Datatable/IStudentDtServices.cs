using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.ViewModel.DataTable;

namespace ServicesLayer.Interface.Datatable
{
    public interface IStudentDtServices
    {
        public StudentDtServicesModal ResponseTable(DtParameters dtParameters, bool gender);
        public StudentDtServicesModal ResponseTable(string text);
    }
}
