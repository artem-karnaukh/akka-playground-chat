using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkkaPlaygrond.Web.Models
{
    public class UserContactModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}