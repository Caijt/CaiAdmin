using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Sso
{
    public class UserDetailDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string CName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string CreatorUserName { get; set; }

        public List<Position> Positions { get; set; }
    }


    public class Position
    {

        public Guid PositionId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }

        public string PositionName { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string JobName { get; set; }

        public bool IsApproval { get; set; }
    }
}
