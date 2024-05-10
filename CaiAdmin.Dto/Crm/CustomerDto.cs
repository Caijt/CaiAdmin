using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string ZhName { get; set; }
        public string Address { get; set; }
        public string EnAddress { get; set; }
        public string ZhAddress { get; set; }
        public string NameUniqueValue { get; set; }
        public string NameLocalizationUniqueValue { get; set; }
        public string LocalizationText { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public Guid? MergerId { get; set; }

        public string Code { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string ExamineState { get; set; }
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}
