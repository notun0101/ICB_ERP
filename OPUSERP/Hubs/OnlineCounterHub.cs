using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OPUSERP.Data;

namespace OPUSERP.Hubs
{
	public class OnlineCounterHub:Hub
	{
		private readonly ERPDbContext _context;
		private readonly HttpContext _httpContext;
		static long counter = 0;

		public OnlineCounterHub(ERPDbContext context, HttpContext httpContext)
		{
			_context = context;
			_httpContext = httpContext;
		}

		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public override async Task OnConnectedAsync()
		{
			var username = _httpContext.User.Identity.Name;
			var remoteIpAddress = _httpContext.Connection.RemoteIpAddress;
			var localIpAddress = _httpContext.Connection.LocalIpAddress;

			await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var username = _httpContext.User.Identity.Name;
			var remoteIpAddress = _httpContext.Connection.RemoteIpAddress;
			var localIpAddress = _httpContext.Connection.LocalIpAddress;

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
			await base.OnDisconnectedAsync(exception);
		}
	}
}
