using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LongRunningProcess {
	/// <summary>
	/// Summary description for DoMuchWork
	/// </summary>
	public class DoMuchWork : IHttpHandler {
		private readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<LogHub>();
		public void ProcessRequest(HttpContext context) {
			context.Response.ContentType = "text/plain";
			var connectionId = context.Request.QueryString["ConnectionId"];
			for (int i = 0; i < 100; i++) {
				Log(connectionId, "Progress: " + i.ToString());
				Thread.Sleep(1000);
			}
		}

		public void Log(string connectionId, string message) {
			var client = _hubContext.Clients.Client(connectionId);
			client.log(message);
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}