using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Entity.Sys;

namespace CaiAdmin.Service.Common
{
    public class CommonService
    {
        private readonly AppDbContext _dbContext;
        private readonly AuthContext _identityContext;
        public CommonService(AppDbContext appDbContext, AuthContext identityContext)
        {
            _dbContext = appDbContext;
            _identityContext = identityContext;
        }

        public List<T> GetPage<T>(IEnumerable<T> query, int pageIndex, int pageSize, bool isC = false)
        {
            if (pageIndex == 0 && pageSize == 0)
            {
                return query.ToList();
            }
            int skipTotal;
            if (isC)
            {
                skipTotal = pageIndex;
            }
            else
            {
                pageIndex -= 1;
                if (pageIndex < 0) pageIndex = 0;
                skipTotal = pageIndex * pageSize;
            }
            return query.Skip(skipTotal).Take(pageSize).ToList();
        }
    }

}
