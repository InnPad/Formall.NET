using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Reflection;

    public abstract class AdoDocumentContext
    {
        private readonly DataSet _dataSet;

        public AdoDocumentContext()
        {
            _dataSet = new DataSet();
        }

        protected DataSet DataSet
        {
            get { return _dataSet; }
        }

        internal protected abstract IDataAdapter CreateDataAdapter(Model model);

        internal protected abstract IDataReader CreateDataReader(Model model);

        internal protected abstract IDbCommand CreateSelectCommand(Model model);

        internal protected abstract IDbCommand CreateUpdateCommand(Model model);

        internal protected abstract IDbCommand CreateDeleteCommand(Model model);

        internal protected abstract IDbCommand CreateInsertCommand(Model model);
    }
}
