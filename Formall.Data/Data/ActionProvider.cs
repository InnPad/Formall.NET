using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Data
{
    internal class ActionProvider : IDataServiceActionProvider
    {
        public bool AdvertiseServiceAction(System.Data.Services.DataServiceOperationContext operationContext, ServiceAction serviceAction, object resourceInstance, bool resourceInstanceInFeed, ref Microsoft.Data.OData.ODataAction actionToSerialize)
        {
            throw new NotImplementedException();
        }

        public IDataServiceInvokable CreateInvokable(System.Data.Services.DataServiceOperationContext operationContext, ServiceAction serviceAction, object[] parameterTokens)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServiceAction> GetServiceActions(System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServiceAction> GetServiceActionsByBindingParameterType(System.Data.Services.DataServiceOperationContext operationContext, ResourceType bindingParameterType)
        {
            throw new NotImplementedException();
        }

        public bool TryResolveServiceAction(System.Data.Services.DataServiceOperationContext operationContext, string serviceActionName, out ServiceAction serviceAction)
        {
            throw new NotImplementedException();
        }
    }
}
