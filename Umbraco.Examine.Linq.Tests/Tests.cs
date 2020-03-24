using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Models;
using Umbraco.Examine.Linq.Extensions;

namespace Umbraco.Examine.Linq.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void DoSomething()
        {
            StringBuilder query = new StringBuilder();
            var index = new Index<Result>(new TestSearcher());
            var results = index.Where(r =>
                !(r.Name.Contains("content page")).Fuzzy(0.2).Boost(10)
                && !(r.Name.ContainsAll("something", "else", "hello").Boost(10) || r.Name != "home boo")
                ).Skip(2).Take(20).ToList();

            //IEnumerable<Result> results = (from r in index.AsQueryable()
            //                               where r.Name.Contains("boo").Boost(10)
            //                               && (r.Name.ContainsAny("something", "else", "hello").Boost(1)
            //                                   || r.CreateDate < DateTime.Now)
            //                               select r).ToList();
        }

        private string GetString()
        {
            return "hello";
        }
    }
}
