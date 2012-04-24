using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityBinderSampleWeb.Models;

namespace EntityBinderSampleWeb.Controllers
{
    public class SampleController : Controller
    {
		public ActionResult Get([EntityBinder()] Entity entity) {
			return null;
		}

    }
}
