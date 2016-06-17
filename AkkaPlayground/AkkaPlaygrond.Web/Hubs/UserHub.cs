using Akka.Actor;
using AkkaPlaygrond.Web.Actors;
using AkkaPlaygrond.Web.Models;

using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Messages.Messages;
using AkkaPlayground.Messages.ReadModels;
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
        //public void JoinSignalRChatGroups(Guid userId)
        //{
        //    UserChatsResult result = GetUserChats(userId);
        //    foreach(var chat in result.Chats)
        //    {
        //        JoinSignalRGroup(chat.ChatId);
        //    }
        //}     

        //public Guid? GetUserChat(Guid currentUserId, Guid targetUserId)
        //{
        //    UserPrivateChatReult userChat = SystemActors.SignalRActor.Ask<UserPrivateChatReult>(new GetPrivateChatWithUser(currentUserId, targetUserId)).Result;

        //    if (userChat.ChatId.HasValue)
        //    {
        //        JoinSignalRGroup(userChat.ChatId.Value);
        //    }

        //    return userChat.ChatId;
        //}

        //public Guid CreateChat(Guid currentUserId, Guid targetUserId)
        //{
        //    Guid chatId = Guid.NewGuid();

        //    var command = new CreateChatCommand(chatId, new List<Guid>() { currentUserId, targetUserId });
        //    ChatCreatedEvent chatCreated = SystemActors.SignalRActor.Ask<ChatCreatedEvent>(command).Result;
        //    JoinSignalRGroup(chatCreated.Id);

        //    return chatCreated.Id;
        //}

        //public void AddChatMessage(Guid currentUserId, Guid chatId, string message)
        //{
        //    SystemActors.SignalRActor.Tell(new AddMessageToChat(chatId, currentUserId, message));
        //}

        //public ChatHistoryResult GetChatLog(Guid chatId)
        //{
        //    GetChatHistory message = new GetChatHistory(chatId);
        //    ChatHistoryResult result = SystemActors.SignalRActor.Ask<ChatHistoryResult>(message).Result;
        //    return result;
        //}

        //public UserChatsResult GetUserChats(Guid userId)
        //{
        //    UserChatsResult result = SystemActors.SignalRActor.Ask<UserChatsResult>(new GetUserChats(userId)).Result;
        //    return result;
        //}

        public void JoinSignalRGroup(Guid groupId)
        {
            Groups.Add(Context.ConnectionId, groupId.ToString());
        }

    }
}