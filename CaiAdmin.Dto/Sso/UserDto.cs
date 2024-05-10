using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Sso
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public bool IsValid { get; set; }
        public bool IsDeleted { get; set; }

        public string CName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string CreatorUserName { get; set; }


    }

}
