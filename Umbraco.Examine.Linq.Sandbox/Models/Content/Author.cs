
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
	 [NodeTypeAlias("Author")]
 	public partial class Author : UmbracoContent
	{
		public override string ContentTypeAlias { get { return "Author"; } }

				
		
				
		
		[Field("name")]
		public string Name { get; set; } 
		
		private IEnumerable<Author> _children = null;
		public IEnumerable<Author> Children
		{
			get
			{
				if (_children == null && this.Content != null)
					_children = this.Content.Children.Select(x => new Author(x));

				return _children;
			}
		}

		public Author()
			: base()
		{
		}

		public Author(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public Author(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public Author(int contentId)
			: base(contentId)
		{
		}

		public Author(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
						
			this.Name = Content.GetPropertyValue<string>("name");
			
		}

	}
}

