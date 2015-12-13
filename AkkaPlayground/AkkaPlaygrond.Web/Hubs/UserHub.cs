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

namespace AkkaPlaygrond.Web.Hubs
{
    [HubName("userHub")]
    public class UserHub : Hub
    {
        public Guid Register(RegisterModel model)
        {
            Guid userId = Guid.NewGuid();
            model.UserId = userId;
            SystemActors.SignalRActor.Tell(model, ActorRefs.Nobody);
            return userId;
        }

        public UserFound Login(string login)
        {
            object result = SystemActors.SignalRActor.Ask(new GetUserByLogin(login)).Result;
            if (result is UserNotFound)
            {
                throw new ArgumentNullException();
            }
            return (UserFound)result;
        }

        public SubscribedToUserEvent AddToContactList(Guid userId, Guid targetUserId)
        {
            SubscribeToUserCommand command = new SubscribeToUserCommand(userId, targetUserId);
            SubscribedToUserEvent result = SystemActors.SignalRActor.Ask<SubscribedToUserEvent>(command).Result;
            return result;
        }

        public List<UserFoundModel> Search(Guid? currentUserId, string searchString)
        {
            var searchTask = SystemActors.SignalRActor.Ask<UserSearchResult>(new GetUsersBySearchString(searchString));
            UserSearchResult usersFound = searchTask.Result;

            List<UserFoundModel> result =  usersFound.Users.Select(x => new UserFoundModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name
                }).ToList();

            if (currentUserId.HasValue)
            {
                SubscribedToListResult userContacts = 
                    SystemActors.SignalRActor.Ask<SubscribedToListResult>(new GetUserSubscribedToList(currentUserId.Value)).Result;
                List<Guid> userContactIds = userContacts.SubscribedToList.Select(x => x.Id).ToList();
                foreach(var item in result)
                {
                    item.IsAlreadyAdded = userContactIds.Contains(item.Id);
                    item.IsCurrentUser = item.Id == currentUserId;
                }
            }

            return result;
        }

        public List<UserContactReadModel> GetUsersContacts(Guid userId)
        {
            SubscribedToListResult userList = SystemActors.SignalRActor.Ask<SubscribedToListResult>(new GetUserSubscribedToList(userId)).Result;
            return userList.SubscribedToList;
        }

        public Guid? GetUserChat(Guid currentUserId, Guid targetUserId)
        {
            UserPrivateChatReult userChat = SystemActors.SignalRActor.Ask<UserPrivateChatReult>(new GetPrivateChatWithUser(currentUserId, targetUserId)).Result;

            if (userChat.ChatId.HasValue)
            {
                JoinSignalRGroup(userChat.ChatId.Value);
            }

            return userChat.ChatId;
        }

        public Guid CreateChat(Guid currentUserId, Guid targetUserId)
        {
            Guid chatId = Guid.NewGuid();

            var command = new CreateChatCommand(chatId, new List<Guid>() { currentUserId, targetUserId });
            ChatCreatedEvent chatCreated = SystemActors.SignalRActor.Ask<ChatCreatedEvent>(command).Result;
            JoinSignalRGroup(chatCreated.Id);

            return chatCreated.Id;
        }

        public void AddChatMessage(Guid currentUserId, Guid chatId, string message)
        {
            SystemActors.SignalRActor.Tell(new AddMessageToChat(chatId, currentUserId, message));
        }


        private void JoinSignalRGroup(Guid chatId)
        {
            Groups.Add(Context.ConnectionId, chatId.ToString());
        }
    }
}