using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web {
	public abstract class ReferenceModel {
		public int Id { get; set; }
	}

	public class ArticleModel : ReferenceModel {
		public string ArticleName { get; set; }
	}

	public class WebPageModel : ReferenceModel {
		public string Url { get; set; }
	}

	public static class DAL {
		private static readonly IList<ReferenceModel> _data = new List<ReferenceModel>
		                                                      {
				new ArticleModel {ArticleName = "On the meaning of life", Id = 1},
				new WebPageModel {Url = "http://www.meaningoflife.com", Id = 2}
		    };

		public static IEnumerable<ReferenceModel> GetData() {
			return _data;
		} 
	}
}