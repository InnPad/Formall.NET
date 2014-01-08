using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Linq
{
    using Formall.Reflection;

    internal class Dictionary
    {
        private readonly DataColumn[] _dataColumns;
        private readonly DataRow _dataRow;
        private readonly Model _model;

        public Dictionary(DataRow dataRow, Model model)
        {
            _dataRow = dataRow;
            _dataColumns = null;
            _model = model;
        }

        protected DataColumn[] DataColumns
        {
            get { return _dataColumns; }
        }

        protected DataRow DataRow
        {
            get { return _dataRow; }
        }
    }
}
