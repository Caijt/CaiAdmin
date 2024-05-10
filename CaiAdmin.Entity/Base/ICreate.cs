using CaiAdmin.Entity.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CaiAdmin.Entity
{
    public interface ICreateUserId
    {
        int CreateUserId { get; set; }

        User CreateUser { get; set; }
    }
    public interface ICreateTime
    {         
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        DateTime CreateTime { get; set; }
    }
    public interface ICreate : ICreateTime, ICreateUserId
    {

    }
    public abstract class CreateUserIdEntity
    {
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public int CreateUserId { get; set; }

        [Navigate(NavigateType.ManyToOne, nameof(CreateUserId))]
        public User CreateUser { get; set; }
    }

    public abstract class CreateTimeEntity
    {
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }
    }

    public abstract class Create : CreateTimeEntity
    { 
            
    }
}
