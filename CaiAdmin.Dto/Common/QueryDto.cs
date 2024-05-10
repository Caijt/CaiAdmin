using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class QueryDto
    {
        public string OrderField { get; set; }
        public bool? OrderDesc { get; set; }

        /// <summary>
        /// 排序字符串
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 从1开始，当PageContinuity为False时，代表第几页，当为True时，代表从第几个数据后面开始获取数据
        /// </summary>
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// 是否连续分页面数据
        /// </summary>
        public bool PageContinuity { get; set; }
    }
}
