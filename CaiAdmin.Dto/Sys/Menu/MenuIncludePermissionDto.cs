using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{

    public class MenuIncludePermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public List<MenuPermissionDto> Permissions { get; set; }
    }

    public class MenuPermissionDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
