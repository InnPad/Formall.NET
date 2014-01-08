using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Raven.Json.Linq;
    using Formall.Navigation;

    public static class MetadataExtensions
    {
        public static RavenJObject FromMetadata(this Metadata metadata)
        {
            return new RavenJObject
            {
                { "@id", metadata.Key },
                { "Raven-Entity-Name", metadata.Key.Exclude(1, '/') }
            };
        }

        public static Metadata ToMetadata(this RavenJObject obj)
        {
            var metadata = new Metadata
            {
                Key = obj.Value<string>("@id")
                //Type = obj.Value<string>("Raven-Entity-Name")
            };

            return metadata;
        }
    }
}
