using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Dynamo
{
    using Amazon.DynamoDBv2.DocumentModel;
    using Amazon.DynamoDBv2.Model;
    using DynamoDBDocument = Amazon.DynamoDBv2.DocumentModel.Document;

    internal class Document : IDocument
    {
        public static implicit operator DynamoDBDocument(Document document)
        {
            return document != null ? document._document : null;
        }

        private readonly DynamoDBDocument _document;
        private readonly Model _model;

        public Document(DynamoDBDocument document, Model model)
        {
            _document = document;
            _model = model;
        }

        #region - ICollection -

        void ICollection<KeyValuePair<string, IEntry>>.Add(KeyValuePair<string, IEntry> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, IEntry>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, IEntry>>.Contains(KeyValuePair<string, IEntry> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, IEntry>>.CopyTo(KeyValuePair<string, IEntry>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<string, IEntry>>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<string, IEntry>>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<string, IEntry>>.Remove(KeyValuePair<string, IEntry> item)
        {
            throw new NotImplementedException();
        }

        #endregion - ICollection -

        #region - IDictionary -

        public Model Model
        {
            get { return _model; }
        }

        void IDictionary<string, IEntry>.Add(string key, IEntry value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, IEntry>.ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        ICollection<string> IDictionary<string, IEntry>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionary<string, IEntry>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, IEntry>.TryGetValue(string key, out IEntry value)
        {
            throw new NotImplementedException();
        }

        ICollection<IEntry> IDictionary<string, IEntry>.Values
        {
            get { throw new NotImplementedException(); }
        }

        IEntry IDictionary<string, IEntry>.this[string key]
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

        #endregion - IDictionary -

        #region - IDocument -

        #endregion - IDocument -

        #region - IEntry -

        DataType IEntry.Type
        {
            get { return _model; }
        }

        #endregion - IEntry -

        #region - IEnumerable -

        IEnumerator<KeyValuePair<string, IEntry>> IEnumerable<KeyValuePair<string, IEntry>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion - IEnumerable -

        #region - IValidatable -

        IEnumerable<ValidationResult> IValidatable.Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion - IValidatable -
    }
}