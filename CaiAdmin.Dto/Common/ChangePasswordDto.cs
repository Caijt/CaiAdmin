using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
