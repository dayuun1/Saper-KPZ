﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DB
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } 
        public string Password { get; set; } 
        public bool IsSoundMuted { get; set; } 
        public List<GameResult> GameResults { get; set; }
    }
}
