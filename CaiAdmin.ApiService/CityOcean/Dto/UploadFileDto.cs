using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public class UploadFileDto
    {
        /// <summary>
        /// 待上传的文件流
        /// </summary>
        public Stream File { get; set; }

        /// <summary>
        /// 是否保存PDF
        /// </summary>
        public bool SavePdf { get; set; }

        /// <summary>
        /// 自定义上传名称
        /// </summary>
        public string FileName { get; set; }
    }
}
