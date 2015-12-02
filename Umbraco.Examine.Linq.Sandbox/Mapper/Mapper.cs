using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Examine;
using Umbraco.Examine.Linq;
using Umbraco.Web;

namespace Umbraco.Examine.Linq.Sandbox.Mapper
{
    public class Mapper<T> : IMapper<T>
    {
        private UmbracoHelper Umbraco;

        public Mapper(UmbracoHelper umbraco)
        {
            Umbraco = umbraco;
        }

        public IEnumerable<T> Map(IEnumerable<SearchResult> results)
        {
            return results.Select(r => (T)Activator.CreateInstance(typeof(T), Umbraco.TypedContent(r.Id)));
        }
    }
}