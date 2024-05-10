using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class RoleMenuDto
    {
        public int MenuId { get; set; }
        public List<string> PermissionCodes { get; set; }
    }
}
