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
            var index = new Index<Result>();
            var results = index.Where(r => 
                r.Name.Contains("boo").Boost(10) 
                && (r.Name.ContainsAny("something", "else", "hello").Boost(10) || r.Name == "home")
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
