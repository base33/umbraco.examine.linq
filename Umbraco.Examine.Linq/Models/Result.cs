using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq.Models
{
    public class Result
    {
        [Field("id")]
        public int Id { get; set; }
        [Field("nodeName")]
        public string Name { get; set; }
        [Field("nodeTypeAlias")]
        public string NodeTypeAlias { get; set; }
        [Field("createDate")]
        public DateTime CreatedDate { get; set; }
        [Field("createDate")]
        public double DateAsInt { get; set; }

        public SearchResult SearchResult { get; set; }
    }
}
