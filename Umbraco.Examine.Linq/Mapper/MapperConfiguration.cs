using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.Mapper
{
    public class MapperConfiguration
    {
        public string SearchResultProperty { get; set; }
        public Dictionary<string, string> FieldMappings { get; set; }

        public MapperConfiguration()
        {
            SearchResultProperty = "";
            FieldMappings = new Dictionary<string, string>();
        }
    }
}
