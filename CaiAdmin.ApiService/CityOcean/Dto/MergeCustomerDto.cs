using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public class MergeCustomerDto
    {
        /// <summary>
        /// 选择合并的客户id
        /// </summary>
        public List<Guid> CustomerIds { get; set; }

        /// <summary>
        /// 保留的客户ID
        /// </summary>
        public Guid KeepCustomerId { get; set; }

        /// <summary>
        /// 确认数据
        /// </summary>
        public List<MgergeCustomerConfirm> Data { get; set; } = new List<MgergeCustomerConfirm>();
    }

    public class MgergeCustomerConfirm
    {
        /// <summary>
        /// 关系类型
        /// </summary>
        public CustomerRelationType Type { get; set; }

        public List<MgergeCustomerConfirmItem> Items { get; set; }

    }

    public class MgergeCustomerConfirmItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关系类型
        /// </summary>
        public CustomerRelationType Type { get; set; }

        /// <summary>
        /// 是否保留
        /// </summary>
        public bool IsKeep { get; set; }
    }
}
