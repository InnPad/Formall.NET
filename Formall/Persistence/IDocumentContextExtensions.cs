using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Navigation;
    using Formall.Reflection;
    using Formall.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public static class IDocumentContextExtensions
    {
        const string idProp = "id";
        const string keyProp = "key";
        const string dataProp = "data";
        const string metadataProp = "@metadata";

        #region - Create -

        public static void Create(this IDocumentContext store, JToken data, Model descriptor = null)
        {
            switch (data.Type)
            {
                case JTokenType.Array:
                    Create(store, data as JArray, descriptor);
                    break;

                case JTokenType.Object:
                    Create(store, data as JObject, descriptor);
                    break;
            }
        }

        public static void Create(this IDocumentContext store, JArray data, Model descriptor = null)
        {
            Create(store, data.Values().AsEnumerable().OfType<JObject>(), descriptor);
        }

        public static void Create(this IDocumentContext store, IEnumerable<JObject> package, Model descriptor = null)
        {
            foreach (var record in package)
                Create(store, record, descriptor);
        }

        public static void Create(this IDocumentContext store, JObject record, Model descriptor = null)
        {
            throw new NotImplementedException();
        }

        #endregion - Create -

        #region - Destroy -

        public static void Destroy(this IDocumentContext store, JToken data, Model descriptor = null)
        {
            switch (data.Type)
            {
                case JTokenType.Array:
                    Destroy(store, data as JArray, descriptor);
                    break;

                case JTokenType.Object:
                    Destroy(store, data as JObject, descriptor);
                    break;
            }
        }

        public static void Destroy(this IDocumentContext store, JArray data, Model descriptor = null)
        {
            Destroy(store, data.Values().AsEnumerable().OfType<JObject>(), descriptor);
        }

        public static void Destroy(this IDocumentContext store, IEnumerable<JObject> package, Model descriptor = null)
        {
            foreach (var record in package)
                Destroy(store, record, descriptor);
        }

        public static void Destroy(this IDocumentContext store, JObject record, Model descriptor = null)
        {
            throw new NotImplementedException();
        }

        #endregion - Destroy -

        #region - Import -

        public static void Import(this IDocumentContext context, string fileName, Model descriptor = null)
        {
            Import(context, new FileInfo(fileName), descriptor);
        }

        public static void Import(this IDocumentContext context, FileInfo file, Model descriptor = null)
        {
            JObject record;

            using (var reader = file.OpenText())
            {
                record = JObject.Load(new JsonTextReader(reader));
            }

            if (record != null)
            {
                var data = record[dataProp];

                if (data != null && data.Type == JTokenType.Array)
                    Import(context, data as JArray, descriptor);
                else
                    Import(context, record, descriptor);
            }
        }

        internal static void Import(this IDocumentContext context, JToken data, Model descriptor = null)
        {
            switch (data.Type)
            {
                case JTokenType.Array:
                    Import(context, data as JArray, descriptor);
                    break;

                case JTokenType.Object:
                    Import(context, data as JObject, descriptor);
                    break;
            }
        }

        internal static void Import(this IDocumentContext context, JArray data, Model descriptor = null)
        {
            Import(context, data.AsEnumerable().OfType<JObject>(), descriptor);
        }

        internal static void Import(this IDocumentContext context, IEnumerable<JObject> package, Model descriptor = null)
        {
            foreach (var record in package)
                Import(context, record);
        }

        internal static bool Import(this IDocumentContext context, JObject data, Model descriptor = null)
        {
            var m = data.Value<JObject>("@metadata");

            var serializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new DefaultContractResolver()
            };

            string key;
            var metadata = (Metadata)serializer.Deserialize(new JTokenReader(m), typeof(Metadata));

            if (metadata != null)
            {
                data.Remove("@metadata");
            }
            else if ((key = data.Value<string>(keyProp)) != null)
            {
                data.Remove(keyProp);
                metadata = new Metadata { Key = key };
            }

            Guid id;
            if (string.IsNullOrWhiteSpace(metadata.Key) || !Guid.TryParse(metadata.Key.Split('/').Last(), out id))
                return false;

            //record.Add(idProp, new JValue(id));

            //
            // Error:
            //
            IDocument document = context.Import(new JsonEntity(data, metadata));
            //
            // Reason: Implicit convertion between JObject to RavenJObject not working correctly
            //
            // Workaround:
            // Export as a document, use the Content Stream to create a TextReader, user the TextReader to import from the context 

            /*using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                serializer.Serialize(writer, data);
                stream.Seek(0L, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    var document = context.Import(reader);
                    context.Store(ref document);
                }
            }*/

            return true;
        }

        #endregion - Import -
    }
}
