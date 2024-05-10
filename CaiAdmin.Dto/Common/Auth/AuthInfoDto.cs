using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class AuthInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<AuthMenuPermissionDto> MenuPermissions { get; set; }

    }
    public class AuthMenuPermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public bool CanMultipleOpen { get; set; }
        public bool IsPage { get; set; }
        public int? ParentId { get; set; }
        public List<string> PermissionCodes { get; set; }
    }
}
