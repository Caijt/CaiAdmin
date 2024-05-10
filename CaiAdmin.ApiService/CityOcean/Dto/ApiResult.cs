using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public class ApiResult
    {
        public string TargetUrl { get; set; }
        public bool Success { get; set; }
        public Error Error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Result { get; set; }
    }

    public class Error
    {
        public int? Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string ValidationErrors { get; set; }
    }
}
