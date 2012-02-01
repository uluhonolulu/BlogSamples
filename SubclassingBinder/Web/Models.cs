using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web {
	public abstract class ReferenceModel {
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }
	}

	public class ArticleModel : ReferenceModel {
		public string ArticleName { get; set; }
		public override string ToString() {
			return this.ArticleName;
		}
	}

	public class WebPageModel : ReferenceModel {
		public string Url { get; set; }
		public override string ToString() {
			return this.Url;
		}
	}

	public static class DAL {
		private static readonly IList<ReferenceModel> _data = new List<ReferenceModel>
		                                                      {
				new ArticleModel {ArticleName = "On the meaning of life", Id = 1},
				new WebPageModel {Url = "http://www.meaningoflife.com", Id = 2}
		    };

		public static IEnumerable<ReferenceModel> GetData() {
			return _data.OrderBy(m => m.Id);
		} 

		public static void Update(ReferenceModel model) {
			var existingModel = _data.Single(m => m.Id == model.Id);
			_data.Remove(existingModel);
			_data.Add(model);
		}
	}
}