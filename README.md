umbraco.examine.linq has been released as Linq To Examine

## What is Linq To Examine?
This project allows you to query the Lucene indexes using LINQ based on your own classes.  This project comes with a basic Result class which allows you to use this straight out of the box.

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
Above, we are creating a new index, setting the type we wish to query.  See Index constructor section for more information on how you can change the target of your search.  By default, the "ExternalSearcher" is used.  Anyway, then we perform the search using Where, and ToList() at the end that executes the query.

You can also perform multiple where clauses, skip and take as of v1.1
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

IEnumberable<Result> results = new Index<BlogPost>()
 .Where(r.Name.Contains(searchTerm));

if(maxDate != DateTime.MinValue)
{
    results = results.Where(r => r.CreatedDate < maxDate);
}

results = results.Skip(startIndex).Take(amount).ToList();
```

##Installation

Install via Nuget using the command below.  Then add the config manually in step 2.

#####Step 1:

PM> Install-Package LINQToExamine

#####Step 2 (I will automate this as part of step 1 asap):

Instert in > Configuration > Compilation > Assemblies

```XML
<add assembly="System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />

<add assembly="System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
```

##Index Constructor - changing the default 

Constructor  | Description
--------------|--------------
Index<T>(string searcherName") | Set which Umbraco Searcher to use (see UmbracoSettings.config)
Index<T>(ISearcher searcher) | Allows you to set up and target your own Lucene index.  The query is passed to you to execute.  All you need to do is implement the ISearcher interface.

##How to use your own custom classes?
```C#
using Umbraco.Examine.Linq.Attributes

[NodeTypeAlias("BlogPost")] //completely optional!!!
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
####string logic:


Extension method / operator  | Description | Example
--------------|--------------|--------------
== (operator) | Whether the field contains the value | r => r.Name == "foo"
!= (operator) | Whether the field does not contains the value | r => r.Name != "foo"
Contains(term)  | Whether the field contains the value | r => r.Name.Contains("foo")
ContainsAny(term)  | Whether the field contains any of the values | r => r.Name.ContainsAny("foo", "bar", "etc")
ContainsAll(term)  | Whether the field contains all of the values | r => r.Name.ContainsAll("foo", "bar", "etc")
All these support NOT (!)

####bool logic:


Extension method / operator  | Description | Example
--------------|--------------|--------------
== (operator) | Whether the field is true or false | r => r.UmbracoNaviHide == false
!= (operator) | Whether the field is not true or false | r => r.UmbracoNaviHide != false

####int and double logic:


operator  | Description | Example
--------------|--------------|--------------
== (operator) | Whether the field is equal | r => r.Rating == 4
!= (operator) | Whether the field is not equal | r => r.Rating != 4
< (operator) | Whether the field is less than | r => r.Rating < 4
<= (operator) | Whether the field is less than or equal to | r => r.Rating <= 4
> (operator) | Whether the field is greater than | r => r.Rating > 4
>= (operator) | Whether the field is greater than or equal to | r => r.Rating >= 4

####DateTime logic:


operator  | Description | Example
--------------|--------------|--------------
== (operator) | Whether the field is equal | r => r.CreatedDate == date
!= (operator) | Whether the field is not equal | r => r.CreatedDate != date
< (operator) | Whether the field is less than | r => r.CreatedDate < date
<= (operator) | Whether the field is less than or equal to | r => r.CreatedDate <= date
> (operator) | Whether the field is greater than | r => r.CreatedDate > date
>= (operator) | Whether the field is greater than or equal to | r => r.CreatedDate >= date


**More logic extension methods on the way, to help with ranges.**

##Fuzzy
Fuzzy allows you to match words in searches that could be "similar" to your search term but may not match your search term exactly.  For example, you may be accepting a search term from the user but they may have slightly mispelt the word.  Using fuzzy you can help them along their way by doing a fuzzy search that may return results that they user was searching for. 

Example: A user types in "umpraco" instead of "umbraco", we will do a search that will match "umbraco" and will return results that contain "umbraco" in the name
```C#
string term = GetTerm(); //get the search term - your implementation
var index = new Index<BlogPost>();
IEnumerable<BlogPost> results = index.Where(c => c.Name.Contains("umpraco").Fuzzy(0.7)).ToList();
```


##Boosting
Boosting allows you to boosts results that meet a specific condition.  For example, you may want to boost the score for results where the content contains the word "Umbraco".  Alternatively, you may want to boost results where the content contains the word "Umbraco" or summary contains "Events".  Lets look at implementing both:

First Query: Get results where the name contains the term, and boost where content contains the word "Umbraco"
```C#
string term = GetTerm(); //get the search term - your implementation
var index = new Index<BlogPost>();
IEnumerable<BlogPost> results = index.Where(c => c.Name.Contains(term) 
                                              && c.Content.Contains("Umbraco").Boost(10)
                                            ).ToList();
```

Second Query: Get results where the name contains the term, and boost where the content contains the word "Umbraco" or the summary contains "Event":
```C#
string term = GetTerm(); //get the search term - your implementation
var index = new Index<BlogPost>();
IEnumerable<BlogPost> results = index.Where(c => c.Name.Contains(term) 
                                  && (c.Content.Contains("Umbraco") || c.Summary.Contains("event")).Boost(10)
                                ).ToList();
```

##Wildcard Searches
Wildcard searches are easy to do.  Just specify the * in the text you are querying.

For example:
```C#
@using Umbraco.Examine.Linq
@using Umbraco.Examine.Linq.Extensions

var index = new Index<Result>();
IEnumberable<Result> results = index.Where(c => c.Name.Contains("umbr*") && c.NodeTypeAlias == "textpage").ToList();
```

##New in 1.0.4
- Ability to query on DateTimes, ints and doubles.

##New in 1.1
- Support for multiple where clauses
- Support for Skip
- Support for Take
