using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fcm
{
    public class BillOfLadingDto
    {
        public Guid Id { get; set; }
        public string No { get; set; }

        public ReleaseType ReleaseType { get; set; }

        /// <summary>
        /// 是否主提单
        /// </summary>
        public bool IsMaster { get; set; }
    }
}
