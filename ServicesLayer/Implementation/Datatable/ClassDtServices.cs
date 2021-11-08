using ServicesLayer.Interface;
using ServicesLayer.ViewModel.DataTable;
using ServicesLayer.Implementation.Datatable;
using ServicesLayer.Interface.Datatable;

using ServicesLayer.ExtensionMethod;
using System.Linq;
using System.Collections.Generic;
using System;
using ServicesLayer.ViewModel;

namespace ServicesLayer.Implementation
{

    public class ClassDtServices : GenericDtServices<ClassDetailServicesModel>, IClassDtServices
    {

        private readonly IClassServices classServices;
        public ClassDtServices(IClassServices classServices)
        {
            this.classServices = classServices;
        }
        public ClassDtServicesModal ResponseTable(DtParameters dtParameters, int min, int max, string quantityName)
        {
            var keySearch = dtParameters.Search.Value ?? string.Empty;
            var filter = classServices.GetClassListDetail(keySearch, dtParameters.Start, dtParameters.Length, min, max, quantityName);
            var result = SortedResult(filter, GetSortedColumns(dtParameters));

            var totalRecords = classServices.GetCounting();
            var recordFiltered = filter.Count;

            return new ClassDtServicesModal()
            {
                draw = dtParameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = totalRecords,
                data = result
            };
        }
    }
}
