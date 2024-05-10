using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class BillCdcDto : BillDto
    {
        /// <summary>
        /// cdc记录时间
        /// </summary>
        public DateTime CdcTime { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public CdcOperationType OperationType { get; set; }
    }


}
