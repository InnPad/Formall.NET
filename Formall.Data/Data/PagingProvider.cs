using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Data
{
    internal class PagingProvider : IDataServicePagingProvider
    {
        public object[] GetContinuationToken(System.Collections.IEnumerator enumerator)
        {
            throw new NotImplementedException();
        }

        public void SetContinuationToken(IQueryable query, ResourceType resourceType, object[] continuationToken)
        {
            throw new NotImplementedException();
        }
    }
}
