using Akka.Persistence;
using AkkaPlayground.Core.Entities;
using System.Collections.Generic;
using Akka;
using System.Linq;
using Akka.Actor;
using System;
using AkkaPlayground.Messages.Events;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Messages;

namespace AkkaPlayground.Core.Actors
{
    public class UserIndex : ReceivePersistentActor
    {
        private List<UserEntity> users = new List<UserEntity>();

        public override string PersistenceId
        {
            get { return "user-index"; }
        }

        private string userIndexViewName = "user-index-view";

        public UserIndex()
        {
            Context.Become(Initialized);
            RecoverAny(UpdateState);

            Context.System.EventStream.Subscribe(Self, typeof(UserRegisteredEvent));
            Context.System.EventStream.Subscribe(Self, typeof(UserNameEmailChangedEvent));
        }

        private void Initialized(object message)
        {
            message.Match()
                .With<UserRegisteredEvent>(register =>
                {
                    Persist(register, UpdateState);
                    
                })
                .With<UserNameEmailChangedEvent>(changeUser =>
                {
                    Persist(changeUser, UpdateState);
                })
                .With<GetUserByEmail>(register =>
                {
                    UserEntity user = users.FirstOrDefault(x => x.Email == register.Email);
                    if (user != null)
                    {
                        Sender.Tell(new UserFound(user.Id, user.Name, user.Email), Self);
                    }
                })
                .With<GetUserById>(u =>
                {
                    UserEntity user = users.FirstOrDefault(x => x.Id == u.UserId);
                    if (user != null)
                    {
                        Sender.Tell(new UserFound(user.Id, user.Name, user.Email), Self);
                    }
                })
                .With<GetUserByLogin>(u =>
                {
                    UserEntity user = users.FirstOrDefault(x => x.Name == u.Login);
                    if (user != null)
                    {
                        Sender.Tell(new UserFound(user.Id, user.Name, user.Email), Self);
                    } 
                    else
                    {
                        Sender.Tell(new UserNotFound(u.Login));
                    }
                })
                .With<GetUsersBySearchString>(u =>
                {
                    IEnumerable<UserEntity> filteredUsers =
                        users.Where(x => x.Name.Contains(u.SearchString) || x.Email.Contains(u.SearchString));
                    IEnumerable<UserFound> usersResult = filteredUsers.Select(x => new UserFound(x.Id, x.Name, x.Email));

                    UserSearchResult result = new UserSearchResult(usersResult.ToList());

                    Sender.Tell(result);
                });

        }

        private void UpdateState(object e)
        {
            e.Match().With<UserRegisteredEvent>(x =>
            {
                users.Add(new UserEntity(x.Id, x.Name, x.Email));
                UpdateView();
            });

            e.Match().With<UserNameEmailChangedEvent>(x =>
            {
                UserEntity user = users.FirstOrDefault(y => y.Id == x.Id);
                if (user != null)
                {
                    user.Name = x.Name;
                    user.Email = x.Email;
                }
                UpdateView();
                
            });
        }

        private void UpdateView()
        {
            if (!IsRecovering)
            {
                IActorRef actorRef = Context.Child(userIndexViewName);
                actorRef.Tell(new Update());
            }
        }
    }
}
