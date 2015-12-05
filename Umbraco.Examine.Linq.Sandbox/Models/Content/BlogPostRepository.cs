
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

using System;
using Umbraco.Examine.Linq.Sandbox.Models.Media;
using Umbraco.Examine.Linq.Attributes;
using ConcreteContentTypes.Core.Extensions;


namespace Umbraco.Examine.Linq.Sandbox.Models.Content
{
	 [NodeTypeAlias("BlogPostRepository")]
 	public partial class BlogPostRepository : UmbracoContent
	{
		public override string ContentTypeAlias { get { return "BlogPostRepository"; } }

				
		
				
		
		[Field("umbracoNaviHide")]
		public bool HideInBottomNavigation { get; set; } 
		
		private IEnumerable<BlogPost> _children = null;
		public IEnumerable<BlogPost> Children
		{
			get
			{
				if (_children == null && this.Content != null)
					_children = this.Content.Children.Select(x => new BlogPost(x));

				return _children;
			}
		}

		public BlogPostRepository()
			: base()
		{
		}

		public BlogPostRepository(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public BlogPostRepository(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public BlogPostRepository(int contentId)
			: base(contentId)
		{
		}

		public BlogPostRepository(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
						
			this.HideInBottomNavigation = Content.GetPropertyValue<bool>("umbracoNaviHide");
			
		}

	}
}

