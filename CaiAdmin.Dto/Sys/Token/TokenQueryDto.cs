using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class TokenQueryDto : QueryDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserLoginName { get; set; }

        /// <summary>
        /// 是否在缓存禁用中
        /// </summary>
        public bool? InCacheDisabled { get; set; }
    }
}
