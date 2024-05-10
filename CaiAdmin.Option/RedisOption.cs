using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Option
{
    public class RedisOption
    {
        public bool Enable { get; set; }
        public string Connection { get; set; }
        public string InstanceName { get; set; }
        public int Database { get; set; }
    }
}
