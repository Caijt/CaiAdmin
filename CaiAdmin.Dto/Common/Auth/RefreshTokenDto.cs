﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsRemember { get; set; }
    }
}
