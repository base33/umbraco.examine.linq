# Umbraco.Examine.Linq
## What is Umbraco.Examine.Linq
This project allows you to query the Lucene indexes using LINQ expressions.

```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
var results = index.Where(c => c.Name == "u*" && c.NodeTypeAlias == "textpage").ToList();
```
Or alternatively,
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
var results = (from r in index
                where r.Name == "u*" && r.NodeTypeAlias == "textpage"
                select r).ToList();
```
