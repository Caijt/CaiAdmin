using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class CustomerOldIcpDetailDto
    {
        public Guid Id { get; set; }
        public string EName { get; set; }
        public string CName { get; set; }

        public string Code { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string EAddress { get; set; }
        public string CAddress { get; set; }
        public Guid MergerId { get; set; }

        public OldIcpExamineState ExamineState { get; set; }

        public string ExamineStateName
        {
            get
            {
                return CommonHelper.GetDescription(ExamineState);
            }
        }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
