
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

        public string Name { get; set; }

        public string Email { get; set; }

        public List<Guid> SubscribedToList { get; set; }

        public List<Guid> FollowersList { get; set; }

        public List<UserChatEntity> Chats { get; set; }

        public UserEntity(Guid id, string login, string name, string email)
        {
            Id = id;
            Login = login;
            Name = name;
            Email = email;
            SubscribedToList = new List<Guid>();
            FollowersList = new List<Guid>();
            Chats = new List<UserChatEntity>();
        }
    }
}
