## What is Umbraco.Examine.Linq?
This project allows you to query the Lucene indexes using LINQ and your own classes.  This project comes with a basic Result class which allows you to use this straight out of the box.

###Code example
Lets start with some code examples.  Say, I want to get all words that contains "umbraco" and it has to be a text page.  This is how the code would look:
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
var results = index.Where(c => c.Name.Contains("umbraco") && c.NodeTypeAlias == "textpage").ToList();
```
Or alternatively,
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
var results = (from r in index
                where r.Name.Contains("umbraco") && r.NodeTypeAlias == "textpage"
                select r).ToList();
```

##How to use your own custom classes?
```C#
using Umbraco.Examine.Linq.Attributes

[NodeTypeAlias("BlogPost")]
public class BlogPost
{
    [Field("nodeName")]
    public string Name { get; set; }
    [Field("content")]
    public string Content { get; set; }
    [Field("summary")]
    public string Summary { get; set; }
    [Field("blogDate")]
    public DateTime BlogDate { get; set; }
    [Field("createDate")]
    public DateTime CreateDate { get; set; }
    
}

```
