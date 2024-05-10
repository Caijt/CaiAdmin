using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class CustomerTitleDto
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

        public bool FromICP { get; set; }
        public string Code { get; set; }
        public string TFN { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Bank { get; set; }
        public string Bank1 { get; set; }
        public string BankAccount1 { get; set; }
        public string Currency1 { get; set; }
        public string Bank2 { get; set; }
        public string BankAccount2 { get; set; }
        public string Currency2 { get; set; }

        public string Mails { get; set; }
        public bool IsValid { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }

        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

    }
}
