﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Entities
{
    public class User
    {
        public int Id { get; set; }
       
        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
