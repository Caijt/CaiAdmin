using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class MergeCustomerResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string NameUniqueValue { get; set; }
        public Guid? KeepCustomerId { get; set; }
        public List<Guid> CustomerIds { get; set; }
    }
}
