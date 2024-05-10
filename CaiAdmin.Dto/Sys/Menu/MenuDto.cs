using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool IsPage { get; set; }
    }
}
