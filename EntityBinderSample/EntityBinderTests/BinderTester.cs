using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using EntityBinderSampleWeb.Models;
using Ivonna.Framework;
using NHibernate;
using NHibernate.Action;
using NUnit.Framework;
using TypeMock.ArrangeActAssert;
using Ivonna.Framework.Mvc;

namespace EntityBinderTests {
	[TestFixture, Isolated]
	public class BinderTester {
		public void ShouldProvideTheEntityInstanceByItsId() {
			// We don't want to set up an ORM, so we'll just fake ISession using TypeMock Isolator
			var fakeSession = Isolate.Swap.NextInstance<ISession>().WithRecursiveFake();
			var entity = new Entity();
			Isolate.WhenCalled(() => fakeSession.Get((Type) null, null)).WillReturn(entity);

			// Now let's execute a Web request
			var response = new TestSession().Get("/Sample/Get?entityId=1");

			// Check the 
		}
	}
}
