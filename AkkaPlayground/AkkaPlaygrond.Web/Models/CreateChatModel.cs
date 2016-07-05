using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaPlaygrond.Web.Models
{
    public class CreateChatModel
    {
        public Guid UserId { get; set; }

        public Guid TargetUserId { get; set; }

        public Guid ChatId { get; set; }
    }
}