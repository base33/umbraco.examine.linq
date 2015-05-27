using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Lucene.Linq.Attributes;

namespace Umbraco.Lucene.Linq.Models
{
    [RecordType("BlogPost")]
    public class Result
    {
        public string Name { get; set; }
        public string NodeTypeAlias { get; set; }
        public DateTime CreateDate { get; set; }
        public int AnInt { get; set; }
    }
}
