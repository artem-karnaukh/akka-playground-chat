using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.ReadModels
{
    public class UserReadModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<UserContactReadModel> SubscribedToList { get; set; }

        public List<UserContactReadModel> FollowersList { get; set; }

        public UserReadModel(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;

            SubscribedToList = new List<UserContactReadModel>();
            FollowersList = new List<UserContactReadModel>();
        }

    }
}
