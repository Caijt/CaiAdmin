using CaiAdmin.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ItSys.Entity
{
    public abstract class IdEntity:IEntity<int>
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }
    }
}
