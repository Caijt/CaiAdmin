using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Wf
{
    public class WorkflowDto
    {
        public Guid Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string ApplicationUserName { get; set; }
        public long ApplicationUserId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime ApplicationTime { get; set; }

        public string WorkflowCode { get; set; }

        public string WorkflowName { get; set; }

        public WorkflowStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                return CommonHelper.GetDescription(Status);
            }
        }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }

    
}
