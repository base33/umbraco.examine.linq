
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
	 [NodeTypeAlias("AuthorsRepository")]
 	public partial class AuthorsRepository : UmbracoContent
	{
		public override string ContentTypeAlias { get { return "AuthorsRepository"; } }

				
		
				
		
		[Field("authors")]
		public string Authors { get; set; } 
		
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

		public AuthorsRepository()
			: base()
		{
		}

		public AuthorsRepository(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public AuthorsRepository(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public AuthorsRepository(int contentId)
			: base(contentId)
		{
		}

		public AuthorsRepository(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
						
			this.Authors = Content.GetPropertyValue<string>("authors");
			
		}

	}
}

