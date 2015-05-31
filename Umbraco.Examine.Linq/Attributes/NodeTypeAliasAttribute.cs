using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeTypeAliasAttribute : Attribute
    {
        public string Name { get; set; }

        public NodeTypeAliasAttribute(string name)
        {
            Name = name;
        }
    }
}
