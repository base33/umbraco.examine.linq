
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

using Umbraco.Examine.Linq.Attributes;
using ConcreteContentTypes.Core.Extensions;


namespace Umbraco.Examine.Linq.Sandbox.Models.Media
{
	 [NodeTypeAlias("Folder")]
 	public partial class Folder : UmbracoMedia
	{
		public override string ContentTypeAlias { get { return "Folder"; } }

		
		
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

		public Folder()
			: base()
		{
		}

		public Folder(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public Folder(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public Folder(int contentId)
			: base(contentId)
		{
		}

		public Folder(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
			
		}

	}
}

