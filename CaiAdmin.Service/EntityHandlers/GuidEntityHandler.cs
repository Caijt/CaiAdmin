using CaiAdmin.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CaiAdmin.Service.EntityHandlers
{
    public class Guid<TEntity> : EntityHandler<TEntity, IGuid>
    {
        public override void OnUpdating(IGuid entity, List<Expression<Func<IGuid, object>>> excludeOrIncludeFields, bool excludeField)
        {
            if (excludeField)
            {
                excludeOrIncludeFields.Add(e => e.Guid);
            }
        }

        protected override void OnCreating(IGuid entity)
        {
            if (entity.Guid == Guid.Empty)
            {
                entity.Guid = Guid.NewGuid();
            }
        }
    }
}
