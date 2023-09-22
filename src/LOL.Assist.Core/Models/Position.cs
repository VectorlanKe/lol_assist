using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOL.Assist.Core.Models
{
    /// <summary>
    /// 位置信息
    /// </summary>
    public class Position
    {
        public Position(string name,string image, string portrait)
        {
            Name = name;
            Image = image;
            Portrait = portrait;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Portrait
        /// </summary>
        public string Portrait { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Image { get; set; }
    }
}
