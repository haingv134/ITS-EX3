using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesLayer.ViewModel.DataTable
{
    public class TestModal
    {
        public int draw;
    }
    public class DtParameters
    {
        private int draw;
        private DtColumn[] columns;
        private DtOrder[] order;
        private int start;
        private int length;
        private DtSearch search;
        public int Draw { get => draw; set => draw = value; }
        public DtColumn[] Columns { get => columns; set => columns = value; }
        public DtOrder[] Order { get => order; set => order = value; }
        public int Start { get => start; set => start = value; }
        public int Length { get => length; set => length = value; }
        public DtSearch Search { get => search; set => search = value; }
    }

    public class DtColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DtSearch Search { get; set; }
    }

    public class DtOrder
    {
        public int Column { get; set; }
        public DtOrderDir Dir { get; set; }
    }

    public enum DtOrderDir
    {
        Asc,
        Desc
    }

    public class DtSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
}
