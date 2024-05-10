using SqlSugar;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class UserLoginLog : IEntity<long>, ICreateTime
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        public string IpAddress { get; set; }
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        [Navigate(NavigateType.ManyToOne, nameof(UserId))]
        public User User { get; set; }
    }
}
