using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class PageListDto<T>
    {
        public int Total { get; set; }
        public List<T> List { get; set; }
    }
}
