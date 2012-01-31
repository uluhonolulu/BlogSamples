using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web {
	public class SampleController : Controller {
		public ActionResult Index() {
			return View(DAL.GetData());
		}

		public ActionResult Edit(int id) {
			var model = DAL.GetData().SingleOrDefault(m => m.Id == id);
			return View(model);
		}

		[HttpPost]
		public ActionResult UpdateReference() {
			return null;
		}
	}
}