using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sso
{
    public class UserQueryDto : QueryDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CName { get; set; }
        public string Name { get; set; }

        public string IdString { get; set; }

        public long[] Ids { get; set; }
    }
}
