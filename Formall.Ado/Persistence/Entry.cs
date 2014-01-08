using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Linq
{
    using Formall.Reflection;

    internal class Entry
    {
        private readonly DataColumn _dataColumn;
        private DataRow _dataRow;

        public Entry(DataRow dataRow, Field field)
        {
            _dataRow = dataRow;
        }

        public object Value
        {
            get;
            set;
        }
    }
}
