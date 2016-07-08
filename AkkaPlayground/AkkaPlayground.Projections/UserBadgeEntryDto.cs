using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Projections
{
    public class UserBadgeEntryDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ChatId { get; set; }

        public Guid MessageId { get; set; }

        public DateTime Date { get; set; }

    }
}
