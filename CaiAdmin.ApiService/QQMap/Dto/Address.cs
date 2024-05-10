using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.QQMap.Dto
{
    public class Address : ApiResult
    {
        public AddressResult Result { get; set; }
    }
    public class AddressResult
    {
        public string Address { get; set; }
    }
}
