using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Lucene.Linq.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RecordTypeAttribute : Attribute
    {
        public string Name { get; set; }

        public RecordTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
