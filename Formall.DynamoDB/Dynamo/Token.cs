using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Dynamo
{
    using Amazon.DynamoDBv2.DocumentModel;

    abstract class Token : IEntry
    {
        public static implicit operator DynamoDBEntry(Token token)
        {
            return token != null ? token._entry : null;
        }

        public static implicit operator Token(Tuple<DynamoDBEntry, DataType> value)
        {
            return null;
        }

        private readonly DynamoDBEntry _entry;
        private readonly DataType _type;

        protected Token(DynamoDBEntry entry, DataType type)
        {
            _entry = entry;
            _type = type;
        }

        DataType IEntry.Type
        {
            get { return _type; }
        }
    }
}
