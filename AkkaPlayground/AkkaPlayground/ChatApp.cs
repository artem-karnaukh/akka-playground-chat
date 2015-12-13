using Akka.Actor;
using Akka.Persistence.Sqlite;
using Akka.Routing;
using AkkaPlayground.Core.Actors;
using AkkaPlayground.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground
{
    public class ChatApp
    {
        private ActorSystem actorSystem;
        private IActorRef userIndexActor;
        private IActorRef userRouter;

        public void Start()
        {
            //InitializeSystem();
            //RegisterUsers();
            
            //Guid chatId = Guid.Parse("e2f45a7d-9c52-4bf2-84a2-66ef90689df0");
            //string name = "baluba";

            //IActorRef chat = actorSystem.ActorOf(Props.Create(() => new Chat(chatId, name)), chatId.ToString());

            //chat.Tell(new JoinChatCommand(chatId, Guid.Parse("3659243f-659f-4a85-ba46-6cdad9955a41")));
            //chat.Tell(new JoinChatCommand(chatId, Guid.Parse("81e017e6-c2a8-4016-afa7-aad358c415e9")));

            //chat.Tell(new SayToChatCommand(chatId, Guid.Parse("3659243f-659f-4a85-ba46-6cdad9955a41"), "Hello buddy"));
            //chat.Tell(new SayToChatCommand(chatId, Guid.Parse("81e017e6-c2a8-4016-afa7-aad358c415e9"), "Hey, how are you?"));

            //Console.ReadLine();
            //actorSystem.Shutdown();
        }

        private void InitializeSystem()
        {
            actorSystem = ActorSystem.Create("akka-persistance");
            var persistance = SqlitePersistence.Get(actorSystem);


            userIndexActor = actorSystem.ActorOf(Props.Create(() => new UserIndex()), "user-index");
            userRouter = actorSystem.ActorOf(Props.Create<UserBucket>().WithRouter(FromConfig.Instance), "user-buckets");
        }

        private void RegisterUsers()
        {
            List<Guid> guids = new List<Guid>() 
            {
                Guid.Parse("3659243f-659f-4a85-ba46-6cdad9955a41"),
                Guid.Parse("81e017e6-c2a8-4016-afa7-aad358c415e9"),
                Guid.Parse("b032174f-2567-4a35-b60a-00509b3c971d"),
                Guid.Parse("991da83d-d269-4bda-a2e3-32fddaf57f4b"),
                Guid.Parse("45698c4a-e98a-4d25-9107-d5376435bd44")
            };

            foreach (Guid userId in guids)
            {
                userRouter.Tell(new RegisterUserCommand(userId, "Artem", "artem@gmail.com"));
                userRouter.Tell(new ChangeUserNameEmailCommand(userId, "Artem Karnaukh", "olya@gmail.com"));
            }
        }
    
    }
}
