
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
using ConcreteContentTypes.Core.Extensions;


namespace Umbraco.Examine.Linq.Sandbox.Models.Content
{
		public partial class Authors : UmbracoContent
	{
		public override string ContentTypeAlias { get { return "Authors"; } }

				
		
				
		
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

		public Authors()
			: base()
		{
		}

		public Authors(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public Authors(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public Authors(int contentId)
			: base(contentId)
		{
		}

		public Authors(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
						
		}

	}
}

