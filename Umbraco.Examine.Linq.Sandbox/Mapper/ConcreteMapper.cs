using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Examine;
using Umbraco.Examine.Linq.Sandbox.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Models;
using Umbraco.Web;
using ConcreteContentTypes.Core.Extensions;
using ConcreteContentTypes.Core.Interfaces;

namespace Umbraco.Examine.Linq.Sandbox.Mapper
{
    public class ConcreteMapper<T> : IMapper<T> where T : class, IConcreteModel, new()
    {
        public IEnumerable<T> Map(IEnumerable<SearchResult> results)
        {
            return results.Select(r => new UmbracoHelper(UmbracoContext.Current).TypedContent(r.Id).As<T>());
        }
    }
}