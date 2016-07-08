using Akka.Actor;
using AkkaPlaygrond.Web.Actors;
using AkkaPlaygrond.Web.Models;

using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Messages.Messages;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;

namespace AkkaPlaygrond.Web.Hubs
{
    [HubName("userHub")]
    public class UserHub : Hub
    {
        public void JoinSignalRGroup(Guid groupId)
        {
            Groups.Add(Context.ConnectionId, groupId.ToString());
        }
    }
}