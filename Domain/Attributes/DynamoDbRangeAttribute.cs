using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetPoc.Domain.Attributes
{
    internal class DynamoDbRangeAttribute : Attribute
    {
        public DynamoDbRangeAttribute(string value)
        {
            Value = new string[] { value };
        }

        public  DynamoDbRangeAttribute(string[] value)
        {
            Value = value;
        }

        public string[] Value { get; }
    }

}