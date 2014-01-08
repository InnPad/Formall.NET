using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Raven.Abstractions.Data;
    using Raven.Abstractions.Replication;

    internal class DocumentStoreConfig
    {
        public string ApiKey
        {
            get;
            set;
        }

        public List<ApiKeyDefinition> ApiKeys
        {
            get;
            set;
        }

        public string DataDirectory
        {
            get;
            set;
        }

        public bool Embeddable
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ReplicationDocument Replication
        {
            get;
            set;
        }

        public string Secret
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }
    }
}
