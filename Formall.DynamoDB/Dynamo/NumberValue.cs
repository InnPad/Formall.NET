using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Dynamo
{
    using Amazon.DynamoDBv2.DocumentModel;

    internal class NumberValue : AbstractValue
    {
        public NumberValue(DynamoDBEntry entry, DataType type)
            : base(entry, type)
        {
        }
    }
}
