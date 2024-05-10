using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class Token : IEntity<long>, ICreateTime
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessExpire { get; set; }
        public int UserId { get; set; }
        [Navigate(NavigateType.ManyToOne, nameof(UserId))]
        public User User { get; set; }
        public string Ip { get; set; }
        public string RefreshToken { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime RefreshExpire { get; set; }
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }
    }
}
