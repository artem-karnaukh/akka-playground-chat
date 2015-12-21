﻿using Akka.Persistence;
using AkkaPlayground.Core.Entities;
using AkkaPlayground.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using AkkaPlayground.Messages.Messages;
using AkkaPlayground.Messages.ReadModels;

namespace AkkaPlayground.Core.Actors.Views
{
    public class UserView : PersistentView
    {
        private Guid _userId;
        private UserReadModel State = null;
        private IActorRef  userViewActor;

        public override string ViewId
        {
            get { return "user-view-" + _userId.ToString();  }
        }

        public override string PersistenceId
        {
            get { return _userId.ToString(); }
        }

        public UserView(Guid userId)
        {
            _userId = userId;
            userViewActor = Context.System.ActorSelection("user/user-index").ResolveOne(TimeSpan.FromSeconds(10)).Result;
        }

        protected override bool Receive(object @event)
        {
            return @event.Match()
                .With<UserRegisteredEvent>(x =>
                {
                    State = new UserReadModel(x.Id, x.Name, x.Email);
                })
                .With<UserNameEmailChangedEvent>(x =>
                {
                    State.Name = x.Name;
                    State.Email = x.Email;
                })
                .With<SubscribedToUserEvent>(x =>
                {
                    var userFound = userViewActor.Ask<UserFound>(new GetUserById(x.TargetUserId)).Result;
                    var contactUser = new UserContactReadModel() { Id = userFound.Id, Name = userFound.Name, Email = userFound.Email };
                    State.SubscribedToList.Add(contactUser);
                })
                .With<UserAddedToFollowersEvent>(x =>
                {
                    var userFound = userViewActor.Ask<UserFound>(new GetUserById(x.TargetUserId)).Result;
                    var contactUser = new UserContactReadModel() { Id = userFound.Id, Name = userFound.Name, Email = userFound.Email };
                    State.FollowersList.Add(contactUser);
                })
                .With<GetUserSubscribedToList>(mes =>
                {
                    Sender.Tell(new SubscribedToListResult(State.SubscribedToList));
                })
                .With<UserChatMessageAddedEvent>(mes =>
                {
                    UserChatReadModel userChat = State.Chats.FirstOrDefault(x => x.ChatId == mes.ChatId);
                    if (userChat != null)
                    {
                        userChat.LastMessage = mes.Message;
                        userChat.LastMessageDate = mes.Date;
                    }
                })
                .With<UserAddedToChatEvent>(mes =>
                {
                    UserChatReadModel userChat = new UserChatReadModel()
                    {
                        ChatId = mes.ChatId,
                    };
                    foreach (var item in mes.Participants)
                    {
                        var userFound = userViewActor.Ask<UserFound>(new GetUserById(item)).Result;
                        UserChatParticipantReadModel participant = new UserChatParticipantReadModel()
                        {
                            UserId = item,
                            Name = userFound.Name
                        };
                        userChat.Participants.Add(participant);
                    }

                    State.Chats.Add(userChat);

                })
                .With<GetUserChats>(mes =>
                {
                    UserChatsResult result = new UserChatsResult(State.Chats);
                    Sender.Tell(result);
                })
                .WasHandled;
        }
        
    }
}
