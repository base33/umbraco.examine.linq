# umbraco.examine.linq
LINQ to Lucene query provider for Umbraco.

'''C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var mm = new .Index<Umbraco.Examine.Linq.Models.Result>();
var results = mm.Where(c => c.Name == "u*" && (c.NodeTypeAlias == "textpage").Boost(10)).ToList();
'''
