using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Dynamo
{
    using Amazon.DynamoDBv2.DocumentModel;

    internal class StringValue : AbstractValue
    {
        public StringValue(DynamoDBEntry entry, DataType type)
            : base(entry, type)
        {
        }
    }
}
