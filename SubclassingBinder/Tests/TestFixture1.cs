using Ivonna.Framework;
using Ivonna.Framework.Mvc;
using NUnit.Framework;
using Web;

namespace Tests {
	[TestFixture, RunOnWeb]
	public class TestFixture1 {
		[Test]
		public void TestModelSubclassing() {
			var response = new TestSession().Post("/Sample/UpdateReference",
				new {
					ModelType = typeof(ArticleModel).ToString(),
					ArticleName = "New name"
				});
			
			//verify that we have the corect model type
			Assert.IsInstanceOf<ArticleModel>(response.ActionMethodParameters["model"]);
			//verify that we have filled the concrete type specific properties
			var model = (ArticleModel)response.ActionMethodParameters["model"];
			Assert.AreEqual("New name", model.ArticleName);
		}
	}
}
