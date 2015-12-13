using Akka.Persistence;
using AkkaPlayground.Core.Entities;
using AkkaPlayground.Messages.Commands;
using AkkaPlayground.Messages.Events;
using System;
using Akka;
using Akka.Actor;
using System.Collections.Generic;

namespace AkkaPlayground.Core.Actors
{
    public class Chat : ReceivePersistentActor
    {
        private ChatEntity State;

        public Chat(Guid id)
        {
            State = new ChatEntity(id);
            Context.Become(UnInitialized);
            RecoverAny(UpdateState);
        }

        public override string PersistenceId
        {
            get { return State.Id.ToString(); }
        }

        private void UnInitialized(object message)
        {
            message.Match()
                .With<CreateChatCommand>(cmd =>
                {
                    var @event = new ChatCreatedEvent(cmd.Id, cmd.Participants);
                    Persist<ChatCreatedEvent>(@event, UpdateState);
                });
        }

        private void Initialized(object message)
        {
            message.Match()
                .With<AddMessageToChat>(mes =>
                {
                    var messaegeAdded = new ChatMessageAddedEvent(mes.ChatId, mes.Author, mes.Message, DateTime.UtcNow);
                    Persist<ChatMessageAddedEvent>(messaegeAdded, UpdateState);
                });
        }

        private void UpdateState(object e)
        {
            e.Match().With<ChatCreatedEvent>(x =>
            {
                State = new ChatEntity(x.Id);
                State.Participants = x.Participants != null ? x.Participants: new List<Guid>();
                Context.Become(Initialized);
                if (!IsRecovering)
                {
                    PublishChatCreated(x);
                }
                Sender.Tell(x);
            });

            e.Match().With<ChatMessageAddedEvent>(x =>
            {
                var chatLogEntity = new ChatLogEntity(x.Author, x.Message, DateTime.UtcNow);
                State.Log.Add(chatLogEntity);
                if (!IsRecovering)
                {
                    PublishMessageAdded(x);
                    Sender.Tell(x);
                }
            });
        }

        private void PublishChatCreated(ChatCreatedEvent chatCreatedEvent)
        {
            foreach (var user in State.Participants)
            {
                Context.System.ActorSelection("/user/user-buckets/*/" + user.ToString()).Tell(chatCreatedEvent);
            }
        }

        private void PublishMessageAdded(ChatMessageAddedEvent chatMessageAdded)
        {
            foreach(var user in State.Participants)
            {
                if (user != chatMessageAdded.Author)
                {
                    Context.System.ActorSelection("/user/user-buckets/*/" + user.ToString()).Tell(chatMessageAdded);
                }
            }
        }
    }
}
