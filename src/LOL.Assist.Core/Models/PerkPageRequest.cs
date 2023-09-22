using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOL.Assist.Core.Models
{
    public class PerkPageRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 主系id
        /// </summary>
        public int PrimaryStyleId { get; set; }
        /// <summary>
        /// 副系id
        /// </summary>
        public int SubStyleId { get; set; }
        /// <summary>
        /// 选中的符文id 【4-主系符文id、2-副系符文id、3-成长符文id】
        /// </summary>
        public List<int> SelectedPerkIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Current { get; set; } = true;
    }

}
