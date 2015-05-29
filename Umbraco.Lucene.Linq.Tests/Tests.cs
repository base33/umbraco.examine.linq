using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Lucene.Linq.Models;
using Umbraco.Lucene.Linq.Extensions;

namespace Umbraco.Lucene.Linq.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void DoSomething()
        {
            StringBuilder query = new StringBuilder();
            var index = new Index<Result>();
            var results = index.Where(r =>
                r.AnInt <= 2 
                && r.Name.Contains("content page").Fuzzy(0.2).Boost(10) 
                && (r.Name.ContainsAny("something", "else", "hello").Boost(10) || r.Name == "home boo")
                && r.CreateDate > new DateTime(2011, 11, 10)
                && r.CreateDate < DateTime.Now
                ).ToList();

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
