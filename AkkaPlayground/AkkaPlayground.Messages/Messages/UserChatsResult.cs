using AkkaPlayground.Messages.ReadModels;
using System.Collections.Generic;

namespace AkkaPlayground.Messages.Messages
{
    public class UserChatsResult
    {
        public List<UserChatReadModel> Chats { get; protected set; }

        public UserChatsResult(List<UserChatReadModel> chats)
        {
            Chats = chats;
        }
    }
}
