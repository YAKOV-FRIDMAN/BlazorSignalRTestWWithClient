using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSignalRTest.Server.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            //return connection.User?.Identity?.Name;
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
