using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq.Sandbox.Models
{
    [NodeTypeAlias("BlogPost")]
    public class BlogPost
    {
        [Field("nodeName")]
        public string Name { get; set; }
        [Field("createDate")]
        public string CreateDate { get; set; }
        [Field("content")]
        public string Content { get; set; }
    }
}