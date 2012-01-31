using Ivonna.Framework;
using Ivonna.Framework.Mvc;
using NUnit.Framework;

namespace Tests {
	[TestFixture, RunOnWeb]
	public class TestFixture1 {
		[Test]
		public void Test() {
			new TestSession().Get("");
		}
	}
}
