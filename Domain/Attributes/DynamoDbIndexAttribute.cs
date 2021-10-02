using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetPoc.Domain.Attributes
{
    internal class DynamoDbIndexAttribute : Attribute
    {
        public DynamoDbIndexAttribute(string value)
        {
            Value = new string[] { value };
        }

        public DynamoDbIndexAttribute(string[] values)
        {
            Value = Value;
        }

        public string[] Value { get; }
    }
}