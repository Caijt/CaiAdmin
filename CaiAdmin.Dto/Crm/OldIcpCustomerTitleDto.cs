using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class OldIcpCustomerTitleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public OldIcpCustomerTitleType Type { get; set; }
        public string TypeName
        {
            get
            {
                return CommonHelper.GetDescription(Type);
            }
        }
        public string Code { get; set; }
        public string TaxNo { get; set; }
        public string AddressTel { get; set; }
        public string BankAccountNo { get; set; }
        public bool IsValid { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
