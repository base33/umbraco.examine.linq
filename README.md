## What is Umbraco.Examine.Linq?
This project allows you to query the Lucene indexes using LINQ and your own classes.  This project comes with a basic Result class which allows you to use this straight out of the box.

###Code example
Lets start with some code examples.  Say, I want to get all words that contains "umbraco" and it has to be a text page.  This is how the code would look:
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
IEnumberable<Result> results = index.Where(c => c.Name.Contains("umbraco") && c.NodeTypeAlias == "textpage").ToList();
```
Or alternatively,
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
IEnumberable<Result> results = (from r in index
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
    
    public Examine.SearchResult Result { get; set; } //will automatically set the result from Examine
}

```
There are two attributes being applied.  The **NodeTypeAlias** attribute will automatically add the filter to the search, so you will only see results of that type.  The **Field** attribute declares that name of the field in the index, if nothing is specified, it will default to the property name.
The data will be mapped directly from the Examine SearchResult into your custom class.  If you specify a property with the type Examine.SearchResult, it will automatically assign the result to your class, if you wanted access to scores etc.

So, say you wanted to find all *BlogPosts'* where the summary contains the terms "fishing" or "cod".  The query will look like this:
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<BlogPost>();
IEnumerable<BlogPost> results = index.Where(c => c.Name.ContainsAny("fishing", "cod")).ToList();
```
##Expression logic
**string** logic:


Extension Method Name  | Description | Example
--------------|--------------|--------------
Contains  | Whether the field contains the value | r => r.Contains("foo")
ContainsAny  | Whether the field contains any of the values | r => r.ContainsAny("foo", "bar", "etc")
ContainsAll | Whether the field contains all of the values | r => r.ContainsAll("foo", "bar", "etc")
