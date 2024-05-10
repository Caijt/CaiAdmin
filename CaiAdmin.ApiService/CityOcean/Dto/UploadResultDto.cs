using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public class UploadResultDto
    {
        /// <summary>
        /// 文件访问ID
        /// </summary>
        public Guid FileId { get; set; }
        /// <summary>
        /// 文件下载根地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExtensionName { get; set; }
    }
}
