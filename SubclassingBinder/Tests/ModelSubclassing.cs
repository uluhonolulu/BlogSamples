using Ivonna.Framework;
using Ivonna.Framework.Mvc;
using NUnit.Framework;
using Web;

namespace Tests {
	[TestFixture, RunOnWeb]
	public class ModelSubclassing {
		private const string NEW_ARTICLE_NAME = "On the meaning of death";

		[Test]
		public void BinderCreatesArticleModelWithValues() {
			var response = new TestSession().Post("/Sample/UpdateReference",
				new {
					ModelType = typeof(ArticleModel).ToString(),
					ArticleName = NEW_ARTICLE_NAME
				});
			
			//verify that we have the corect model type
			Assert.IsInstanceOf<ArticleModel>(response.ActionMethodParameters["model"]);
			//verify that we have filled the concrete type specific properties
			var model = (ArticleModel)response.ActionMethodParameters["model"];
			Assert.AreEqual(NEW_ARTICLE_NAME, model.ArticleName);
		}
	}
}
