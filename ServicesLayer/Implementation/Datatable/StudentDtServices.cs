using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.Implementation;
using ServicesLayer.Implementation.Datatable;
using ServicesLayer.Interface;
using ServicesLayer.Interface.Datatable;
using ServicesLayer.ViewModel;
using ServicesLayer.ViewModel.DataTable;

namespace ServicesLayer.Implementation.Datatable
{
    public class StudentDtServices : GenericDtServices<StudentDetailServicesModel>, IStudentDtServices
    {
        private readonly IStudentServices studentServices;
        public StudentDtServices(IStudentServices studentServices)
        {
            this.studentServices = studentServices;
        }

        public StudentDtServicesModal ResponseTable(DtParameters dtParameters, string gender, int classId)
        {
            var keysearch = dtParameters.Search.Value ?? string.Empty;
            var filter = studentServices.GetStudentListDetail(keysearch, classId, gender, dtParameters.Start, dtParameters.Length);
            var result = SortedResult(filter, GetSortedColumns(dtParameters));

            var recordTotal = studentServices.GetCounting();
            return new StudentDtServicesModal()
            {
                draw = dtParameters.Draw,
                recordsFiltered = filter.Count,
                recordsTotal = recordTotal,
                data = result
            };
        }
    }
}
