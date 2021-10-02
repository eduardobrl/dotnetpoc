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
            Value = value;
        }

        public string Value { get; }
    }
}