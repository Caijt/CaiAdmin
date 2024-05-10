using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class MenuSaveDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public int? ParentId { get; set; }
        public bool IsPage { get; set; }
        public bool CanMultipleOpen { get; set; }
        public List<int> ApiIds { get; set; }

        public List<MenuSavePermissionSaveDto> Permissions { get; set; }
    }


    public class MenuSavePermissionSaveDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public List<int> ApiIds { get; set; }
    }
}
