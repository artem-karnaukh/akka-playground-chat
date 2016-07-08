using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Projections
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string UserName { get; set; }

    }
}
