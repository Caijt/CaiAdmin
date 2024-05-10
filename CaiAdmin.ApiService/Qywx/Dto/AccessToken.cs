using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.Qywx.Dto
{
    public class AccessToken : ApiResult
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
    }
}
