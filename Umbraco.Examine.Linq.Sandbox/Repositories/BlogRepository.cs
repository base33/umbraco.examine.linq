using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Examine.Linq.Sandbox.Mapper;
using Umbraco.Examine.Linq.Sandbox.Models.Content;
using Umbraco.Examine.Linq.Extensions;

namespace Umbraco.Examine.Linq.Sandbox.Repositories
{
    public class BlogRepository
    {
        public IEnumerable<BlogPost> GetAllBlogPosts()
        {
            return new Index<BlogPost>(new ConcreteMapper<BlogPost>()).Where(c => (c.Author.Id == 1095 || c.Name.Contains("only").Fuzzy(0.7).Boost(10)) && c.CreateDate > DateTime.Now.AddMonths(-10)).Take(2);
        }
    }
}