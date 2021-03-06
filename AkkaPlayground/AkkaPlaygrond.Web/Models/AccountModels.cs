﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Globalization;
using System.Web.Security;

namespace AkkaPlaygrond.Web.Models
{
    public class RegisterModel
    {
        public Guid UserId { get; set; }

        public string Login { get; set; }

        public string UserName { get; set; }
    }
}
