﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class AttachQueryDto : QueryDto
    {
        public Guid? EntityGuid { get; set; }
        public Guid? Guid { get; set; }
        public string Type { get; set; }
    }
}
