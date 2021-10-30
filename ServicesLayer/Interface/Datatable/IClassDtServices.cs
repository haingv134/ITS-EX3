using ServicesLayer.ViewModel;
using ServicesLayer.ViewModel.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Interface.Datatable
{
    public interface IClassDtServices
    {
        public ClassDtServicesModal ResponseTable(DtParameters dtParameters, int min, int max, string quantityName);
    }
}
