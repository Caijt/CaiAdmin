using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Wf
{
    public class WorkflowDetailDto : WorkflowDto
    {
        public List<WorkflowTaskDto> Tasks { get; set; }
    }

    public class WorkflowTaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid WorkflowId { get; set; }

        public DateTime CreationTime { get; set; }

        public TaskStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                return CommonHelper.GetDescription(Status);
            }
        }

        public DateTime? FinishedTime { get; set; }

        public string ExtraProperties { get; set; }

        public List<WorkflowTaskProcessDto> Processes { get; set; }
        public List<WorkflowTaskCandidateDto> Candidates { get; set; }
    }

    public class WorkflowTaskProcessDto
    {
        public Guid Id { get; set; }
        public long ProcessUserId { get; set; }
        public string ProcessUserName { get; set; }

        public Guid TaskId { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime? FinishedTime { get; set; }
        public TaskProcessStatus Status { get; set; }
        public string FormData { get; set; }
        public string ExtensionData { get; set; }
        public string StatusName
        {
            get
            {
                return CommonHelper.GetDescription(Status);
            }
        }
    }

    public class WorkflowTaskCandidateDto
    {
        public Guid Id { get; set; }
        public long CandidateUserId { get; set; }
        public string CandidateUserName { get; set; }

        public Guid TaskId { get; set; }
    }
}
