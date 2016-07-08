
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string UserName { get; set; }

        public List<Guid> SubscribedToList { get; set; }

        public List<Guid> FollowersList { get; set; }

        public List<UserChatEntity> Chats { get; set; }

        public UserEntity(Guid id, string login, string userName)
        {
            Id = id;
            Login = login;
            UserName = userName;
            SubscribedToList = new List<Guid>();
            FollowersList = new List<Guid>();
            Chats = new List<UserChatEntity>();
        }
    }
}
