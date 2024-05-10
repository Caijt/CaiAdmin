using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class ExamineStatusExceptionCustomerDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? ExamineTime { get; set; }
        public ExamineState ExamineState { get; set; }

        public string ExamineStateName
        {
            get
            {
                return CommonHelper.GetDescription(ExamineState);
            }
        }
        public string OldIcpCode { get; set; }
        public OldIcpExamineState OldIcpExamineState { get; set; }

        public string OldIcpExamineStateName
        {
            get
            {
                return CommonHelper.GetDescription(OldIcpExamineState);
            }
        }
    }
}
