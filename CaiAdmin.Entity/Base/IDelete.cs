using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity
{
    public interface IDeleteUserId
    {
        int? DeleteUserId { get; set; }
    }
    public interface IDeleteTime
    {
        DateTime? DeleteTime { get; set; }
    }
    public interface IDelete : IDeleteUserId, IDeleteTime
    { }
}
