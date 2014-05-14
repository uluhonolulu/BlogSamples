using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace LongRunningProcess {
	//required for OWIN to be initialized
	public class Startup {
		public void Configuration(IAppBuilder app) {
			app.MapSignalR();
		}
	}
}