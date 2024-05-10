using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class RoleSaveDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public List<RoleMenuDto> MenuPermissions { get; set; }
    }
}
