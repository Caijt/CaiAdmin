using CaiAdmin.Database;

namespace CaiAdmin.Service
{
    /// <summary>
    /// 查询服务基类
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly Repository _repository;

        /// <summary>
        /// 仓储
        /// </summary>
        protected Repository Repository => _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public BaseService(Repository repository)
        {
            this._repository = repository;
        }
    }

    /// <summary>
    /// 查询服务基类
    /// </summary>
    /// <typeparam name="TQuery">查询对象类型</typeparam>
    public abstract class BaseService<TQuery>
        where TQuery : class, new()
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly Repository<TQuery> _repository;

        /// <summary>
        /// 仓储
        /// </summary>
        protected Repository<TQuery> Repository => _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public BaseService(Repository<TQuery> repository)
        {
            this._repository = repository;
        }
    }
}
