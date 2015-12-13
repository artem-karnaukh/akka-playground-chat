using AkkaPlayground.Messages.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class SubscribedToListResult
    {
        public List<UserContactReadModel> SubscribedToList { get; private set; }

        public SubscribedToListResult(List<UserContactReadModel> subscribedToList)
        {
            SubscribedToList = subscribedToList;
        }
    }
}
