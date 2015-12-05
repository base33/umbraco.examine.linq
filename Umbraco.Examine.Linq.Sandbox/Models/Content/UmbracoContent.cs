
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Web;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ConcreteContentTypes.Core.Models;
using Newtonsoft.Json;
using ConcreteContentTypes.Core.Interfaces;
using ConcreteContentTypes.Core.Models.Enums;
using Umbraco.Core;
using Umbraco.Core.Services;

using ConcreteContentTypes.Core.Extensions;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq.Sandbox.Models.Content
{
		public abstract partial class UmbracoContent : IConcreteModel
	{
		public abstract string ContentTypeAlias { get; }

		private IPublishedContent _content = null;
		[JsonIgnore]
		public IPublishedContent Content
		{
			get
			{
				if (_content == null && this.Id != 0)
					_content = UmbracoContext.Current.ContentCache.GetById(this.Id);

				return _content;
			}
			set
			{
				_content = value;
			}
		}

		[Field("nodeName")]
		public string Name { get; set; }

		[Field("id")]
		public int Id { get; set; }
		
		public int ParentId { get; set; }
		
		public string Path { get; set; }
		
		[Field("createDate")]
		public DateTime CreateDate { get; set; }
		
		[Field("updateDate")]
		public DateTime UpdateDate { get; set; }
		
		public string Url { get; set; }

		#region Constructors and Initalisation

 		public UmbracoContent()
			: base()
 		{
 		}
 
 		public UmbracoContent(int contentId)
 		{
			Init(contentId);
 		}
 
 		public UmbracoContent(IPublishedContent content)
 		{
			Init(content);
 		}

		public void Init(int contentId)
		{
			IPublishedContent content = UmbracoContext.Current.ContentCache.GetById(contentId);

			if (content == null)
				throw new InvalidOperationException(string.Format("Content Id {0} not found in Umbraco Cache", contentId));

			Init(content);
		}

		public virtual void Init(IPublishedContent content)
		{
			this.Content = content;

			this.Name = this.Content.Name;
			this.Id = this.Content.Id;
			this.ParentId = this.Content != null && this.Content.Parent != null ? this.Content.Parent.Id : -1; //TODO: Not sure about this, means we always grab the parent IPublishedContent too...
			this.Path = this.Content.Path;
			this.CreateDate = this.Content.CreateDate;
			this.UpdateDate = this.Content.UpdateDate;
			this.Url = this.Content.Url;
		}

		#endregion

 	}
} 
