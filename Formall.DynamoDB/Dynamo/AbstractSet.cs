using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Dynamo
{
    using Amazon.DynamoDBv2.DocumentModel;

    internal abstract class AbstractSet : Container
    {
        protected AbstractSet(DynamoDBEntry entry, DataType type)
            : base(entry, type)
        {
        }
    }
}
