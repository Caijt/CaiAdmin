﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class LoginDto
    {
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public bool IsRemember { get; set; }

    }
}
