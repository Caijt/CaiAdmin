using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.Qywx.Dto
{
    public class JsapiTicket : ApiResult
    {
        public string Ticket { get; set; }
        public int Expires_in { get; set; }
    }
}
