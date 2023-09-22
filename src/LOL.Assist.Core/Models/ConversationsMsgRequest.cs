using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOL.Assist.Core.Models
{
    /// <summary>
    /// 会话消息发送请求
    /// </summary>
    public class ConversationsMsgRequest
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type { get; set; } = "chat";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}
