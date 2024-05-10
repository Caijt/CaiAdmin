using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Wf
{
    public class WorkflowQueryDto : QueryDto
    {
        public string IdString { get; set; }
        public string No { get; set; }
        public string Name { get; set; }

        public Guid[] Ids { get; set; }

        public WorkflowStatus[] Statuses { get; set; }
        public string[] WorkflowCodes { get; set; }

    }
}
