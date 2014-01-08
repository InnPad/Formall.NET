using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Data
{
    internal class QueryProvider : IDataServiceQueryProvider
    {
        public object CurrentDataSource
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object GetOpenPropertyValue(object target, string propertyName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> GetOpenPropertyValues(object target)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyValue(object target, ResourceProperty resourceProperty)
        {
            throw new NotImplementedException();
        }

        public IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet)
        {
            throw new NotImplementedException();
        }

        public ResourceType GetResourceType(object target)
        {
            throw new NotImplementedException();
        }

        public object InvokeServiceOperation(ServiceOperation serviceOperation, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool IsNullPropagationRequired
        {
            get { throw new NotImplementedException(); }
        }
    }
}
