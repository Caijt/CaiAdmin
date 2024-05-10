using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.Qywx.Dto
{
    public class UserInfo : ApiResult
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public string OpenId { get; set; }
    }
}
