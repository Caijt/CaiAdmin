using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Service.Fam
{
    public class CheckDetailDto
    {
        public Guid Id { get; set; }
        public string No { get; set; }

        public Guid RefId { get; set; }
        public DateTime AccountDate { get; set; }
        public DateTime DueDate { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<BillChargesDto> Charges { get; set; }
        /// <summary>
        /// 工作流
        /// </summary>
        public List<BillWorkflowDto> Workflows { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
