
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Web;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ConcreteContentTypes.Core.Models;
using ConcreteContentTypes.Core.Interfaces;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;
using System;
using Umbraco.Examine.Linq.Sandbox.Models.Media;
using Umbraco.Examine.Linq.Attributes;
using ConcreteContentTypes.Core.Extensions;


namespace Umbraco.Examine.Linq.Sandbox.Models.Content
{
	 [NodeTypeAlias("BlogPost")]
 	public partial class BlogPost : UmbracoContent
	{
		public override string ContentTypeAlias { get { return "BlogPost"; } }

				
		[JsonIgnore]
		public GridContent content { get; set; } 		
		
				
		
		[Field("introduction")]
		public string Introduction { get; set; } 		
		
        
		private IPublishedContent _author = null;

        [Field("author")]
        public IPublishedContent Author
		{
			get 
			{
				if (_author == null)
				{
					int? contentId = Content.GetPropertyValue<int?>("author");

					if (contentId.HasValue)
					{
					
						_author = UmbracoContext.Current.ContentCache.GetById(contentId.Value);
				
						
					}	
				}
				return _author;
			}
		} 
		
		private IEnumerable<IPublishedContent> _children = null;
		[JsonIgnore]
		public IEnumerable<IPublishedContent> Children
		{
			get
			{
				if (_children == null && this.Content != null)
					_children = this.Content.Children;

				return _children;
			}
		}

		public BlogPost()
			: base()
		{
		}

		public BlogPost(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public BlogPost(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public BlogPost(int contentId)
			: base(contentId)
		{
		}

		public BlogPost(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
						
			this.content = new GridContent("content", this.Content);
						
			this.Introduction = Content.GetPropertyValue<string>("introduction");
			
		}

	}
}

