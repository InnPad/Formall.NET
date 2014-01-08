using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Data
{
    internal class StreamProvider2 : IDataServiceStreamProvider2
    {
        public System.IO.Stream GetReadStream(object entity, ResourceProperty streamProperty, string etag, bool? checkETagForEquality, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public Uri GetReadStreamUri(object entity, ResourceProperty streamProperty, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public string GetStreamContentType(object entity, ResourceProperty streamProperty, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public string GetStreamETag(object entity, ResourceProperty streamProperty, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream GetWriteStream(object entity, ResourceProperty streamProperty, string etag, bool? checkETagForEquality, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public void DeleteStream(object entity, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream GetReadStream(object entity, string etag, bool? checkETagForEquality, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public Uri GetReadStreamUri(object entity, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public string GetStreamContentType(object entity, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public string GetStreamETag(object entity, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream GetWriteStream(object entity, string etag, bool? checkETagForEquality, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public string ResolveType(string entitySetName, System.Data.Services.DataServiceOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public int StreamBufferSize
        {
            get { throw new NotImplementedException(); }
        }
    }
}
