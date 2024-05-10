using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class MenuEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public int? ParentId { get; set; }
        public bool IsPage { get; set; }
        public bool CanMultipleOpen { get; set; }
        public string ParentName { get; set; }

        public List<MenuEditApiDto> Apis { get; set; }

        public List<MenuEditPermissionEditDto> Permissions { get; set; }
    }


    public class MenuEditPermissionEditDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public List<MenuEditApiDto> Apis { get; set; }
    }

    public class MenuEditApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

}
