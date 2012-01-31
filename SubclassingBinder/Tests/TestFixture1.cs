using Ivonna.Framework;
using Ivonna.Framework.Mvc;
using NUnit.Framework;
using Web;

namespace Tests {
	[TestFixture, RunOnWeb]
	public class TestFixture1 {
		[Test]
		public void Test() {
			new TestSession().Get("");
		}
		[Test]
		public void TestModelSubclassing() {
			var response = new TestSession().Post("/Sample/UpdateReference",
				new {
					ModelType = "MvcApplication1.CustomModelBinders.ModelSubclassing.ArticleModel"
				});
			Assert.IsInstanceOf<ArticleModel>(response.ActionMethodParameters["model"]);
		}
	}
}
