using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaPlaygrond.Web.Models
{
    public class AddMessageToChatModel
    {
        public Guid ChatId { get; protected set; }

        public Guid Author { get; protected set; }

        public string Message { get; protected set; }
    }
}