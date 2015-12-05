
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
using Umbraco.Examine.Linq.Attributes;
using ConcreteContentTypes.Core.Extensions;


namespace Umbraco.Examine.Linq.Sandbox.Models.Media
{
	 [NodeTypeAlias("Image")]
 	public partial class Image : UmbracoMedia
	{
		public override string ContentTypeAlias { get { return "Image"; } }

				
		
				
		
		[Field("umbracoFile")]
		public string UploadImage { get; set; } 		
		
				
		
		[Field("umbracoWidth")]
		public string Width { get; set; } 		
		
				
		
		[Field("umbracoHeight")]
		public string Height { get; set; } 		
		
				
		
		[Field("umbracoBytes")]
		public string Size { get; set; } 		
		
				
		
		[Field("umbracoExtension")]
		public string Type { get; set; } 
		
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

		public Image()
			: base()
		{
		}

		public Image(string name, IConcreteModel parent)
			: this(name, parent.Id)
		{
		}

		public Image(string name, int parentId)
			: base()
		{
			this.Name = name;
			this.ParentId = parentId;
		}

		public Image(int contentId)
			: base(contentId)
		{
		}

		public Image(IPublishedContent content)
			: base(content)
		{
		}

		public override void Init(IPublishedContent content)
		{
			base.Init(content);
						
			this.UploadImage = Content.GetPropertyValue<string>("umbracoFile");
						
			this.Width = Content.GetPropertyValue<string>("umbracoWidth");
						
			this.Height = Content.GetPropertyValue<string>("umbracoHeight");
						
			this.Size = Content.GetPropertyValue<string>("umbracoBytes");
						
			this.Type = Content.GetPropertyValue<string>("umbracoExtension");
			
		}

	}
}

