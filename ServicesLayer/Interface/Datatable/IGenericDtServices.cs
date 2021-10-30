using ServicesLayer.ViewModel.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.Interface.Datatable
{
    public interface IGenericDtServices<TEntity> where TEntity : class
    {
        public List<TEntity> SortedResult(List<TEntity> list, Dictionary<string, bool> sortedColums);
    }
}
