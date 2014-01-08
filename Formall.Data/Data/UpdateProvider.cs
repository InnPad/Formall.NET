using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Data
{
    internal class UpdateProvider : IDataServiceUpdateProvider
    {
        public void SetConcurrencyValues(object resourceCookie, bool? checkForEquality, IEnumerable<KeyValuePair<string, object>> concurrencyValues)
        {
            throw new NotImplementedException();
        }

        public void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
        {
            throw new NotImplementedException();
        }

        public void ClearChanges()
        {
            throw new NotImplementedException();
        }

        public object CreateResource(string containerName, string fullTypeName)
        {
            throw new NotImplementedException();
        }

        public void DeleteResource(object targetResource)
        {
            throw new NotImplementedException();
        }

        public object GetResource(IQueryable query, string fullTypeName)
        {
            throw new NotImplementedException();
        }

        public object GetValue(object targetResource, string propertyName)
        {
            throw new NotImplementedException();
        }

        public void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
        {
            throw new NotImplementedException();
        }

        public object ResetResource(object resource)
        {
            throw new NotImplementedException();
        }

        public object ResolveResource(object resource)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void SetReference(object targetResource, string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }

        public void SetValue(object targetResource, string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }
    }
}
