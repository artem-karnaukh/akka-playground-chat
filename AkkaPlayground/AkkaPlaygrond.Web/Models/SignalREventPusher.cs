using AkkaPlaygrond.Web.Hubs;
using AkkaPlayground.Messages.Events;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaPlaygrond.Web.Models
{
    public class SignalREventPusher
    {
        private static readonly IHubContext _userHubContext;

        static SignalREventPusher()
        {
            _userHubContext = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
        }

        
        public void PlayerJoined(Guid id, string userName, string email)
        {
            _userHubContext.Clients.All.userJoined(id, userName, email);
        }

        public void ChatMessageAdded(ChatMessageAddedEvent evt)
        {
            _userHubContext.Clients.Group(evt.ChatId.ToString()).chatMessageAdded(evt);
        }
    }
}