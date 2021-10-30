using ServicesLayer.ViewModel.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer.Interface.Datatable;
using ServicesLayer.ExtensionMethod;

namespace ServicesLayer.Implementation.Datatable
{
    public class GenericDtServices<TEntity> : IGenericDtServices<TEntity> where TEntity : class
    {
        // get columns that contain sort contidions (name of culums and directions)
        // column ~ data of columns
        // <colums, orderAsendingDirection>
        public Dictionary<string, bool> GetSortedColumns(DtParameters dtParameters)
        {
            var res = new Dictionary<string, bool>();
            if (dtParameters.Order == null)
            {
                res.Add("ClassName", true);
            }
            else
                for (int index = 0; index < dtParameters.Order.Length; index++)
                {
                    var orderBy = dtParameters.Columns[dtParameters.Order[index].Column].Name;
                    var orderAscendingDirection = dtParameters.Order[index].Dir.ToString().ToLower() == "asc";
                    res.Add(orderBy, orderAscendingDirection);
                }
            return res;
        }
        public List<TEntity> SortedResult(List<TEntity> list, Dictionary<string, bool> sortedColums)
        {
            if (sortedColums.Count == 0) return list;
            int index = 0;
            var query = list.AsQueryable();
            foreach (var dic in sortedColums)
            {
                index++;
                if (index == 1) query = dic.Value ? query.OrderBy(dic.Key) : query.OrderByDescending(dic.Key);
                else
                {
                    query = dic.Value ? query.ThenBy(dic.Key) : query.ThenByDescending(dic.Key);
                }
            }
            return query.ToList();
        }
    }
}
