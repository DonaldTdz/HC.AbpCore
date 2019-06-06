using System;
using System.Collections.Generic;
using System.Text;
using Top.Api.Util;

namespace HC.AbpCore.Common
{
    public class DingMedia
    {
    }

    /// <summary>
    /// 请求参数
    /// </summary>
    public class DingMediaRequest
    {
        /// <summary>
        /// 媒体文件类型，分别有图片（image）、语音（voice）、普通文件(file)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// form-data中媒体文件标识，有filename、filelength、content-type等信息
        /// </summary>
        public FileItem Media { get; set; }
    }

    /// <summary>
    /// 返回参数
    /// </summary>
    public class DingMediaResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public long errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 媒体文件类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 媒体文件上传后获取的唯一标识
        /// </summary>
        public string media_id { get; set; }

        /// <summary>
        /// 媒体文件上传时间戳
        /// </summary>
        public long created_at { get; set; }
    }
}
