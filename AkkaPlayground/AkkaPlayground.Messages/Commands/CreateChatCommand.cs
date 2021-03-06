﻿using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Commands
{
    public class CreateChatCommand 
    {
        public Guid Id { get; private set; }

        public Guid Creator { get; private set; }

        public List<Guid> Participants { get; private set; }

        public CreateChatCommand(Guid id, Guid creator, List<Guid> participants)
        {
            Id = id;
            Creator = creator;
            Participants = participants;
        }

    }
}
