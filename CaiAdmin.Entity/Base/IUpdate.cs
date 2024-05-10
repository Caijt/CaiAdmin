using CaiAdmin.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity
{
    public interface IUpdateUserId
    {
        int UpdateUserId { get; set; }
        User UpdateUser { get; set; }
    }
    public interface IUpdateTime
    {
        DateTime UpdateTime { get; set; }
    }
    public interface IUpdate : IUpdateUserId, IUpdateTime
    {

    }
}
