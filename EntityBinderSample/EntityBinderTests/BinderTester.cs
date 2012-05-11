using EntityBinderSampleWeb.Models;
using Ivonna.Framework;
using NHibernate;
using NUnit.Framework;
using Ivonna.Framework.Mvc;

namespace EntityBinderTests {
	[TestFixture, RunOnWeb]
	public class BinderTester {
		[Test]
		public void ShouldProvideTheEntityInstanceByItsId() {
			var entity = new Entity();
			// We don't want to set up an ORM, 
			// so we'll just fake ISession
			var session = new TestSession();
			session.Stub<ISession>("Get").Return(entity);

			// Now let's execute a Web request
			var response = session.Get("/Sample/Get?entityId=1");

			// Check the result
			Assert.AreEqual(entity, response.ActionMethodParameters["entity"]);
		}
	}
}
