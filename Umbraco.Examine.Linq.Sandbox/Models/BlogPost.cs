﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq.Sandbox.Models
{
    [NodeTypeAlias("BlogPost")]
    public class BlogPost
    {
        [Field("id")]
        public int Id { get; set; }
        [Field("nodeName")]
        public string Name { get; set; }
        [Field("createDate")]
        public DateTime CreatedDate { get; set; }
        [Field("content")]
        public string Content { get; set; }

        public BlogPost(IPublishedContent content)
        {
            Id = content.Id;
            Name = content.Name;
            CreatedDate = content.CreateDate;
            Content = "Bla";
        }
    }
}