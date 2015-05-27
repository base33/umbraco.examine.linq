using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Lucene.Linq.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
        
        public FieldAttribute(string name)
        {
            this.Name = name;
        }
    }
}
