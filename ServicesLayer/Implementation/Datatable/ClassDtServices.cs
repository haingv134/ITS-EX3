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
            
            // filter result basr on classPart and StudentPart
            var filter = classServices.FilterByText(keySearch);    
            
            var result = classServices.GetClassListDetailWithRangeCondition(filter, min, max, dtParameters.Start, dtParameters.Length,quantityName);              
            result = SortedResult(result, GetSortedColumns(dtParameters));

            var totalRecords = classServices.GetCounting();
            var recordFiltered = filter.Count;

            return new ClassDtServicesModal()
            {
                draw = dtParameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = totalRecords,
                data = result.ToList()
            };
        }
    }
}
