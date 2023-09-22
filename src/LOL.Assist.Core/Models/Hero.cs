namespace LOL.Assist.Core.Models
{
    public class Hero
    {
        /// <summary>
        /// id：1
        /// </summary>
        public int HeroId { get; set; }
        /// <summary>
        /// 名称： 黑暗之女
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 别名： Annie
        /// </summary>
        public string Alias { get; set; } = string.Empty;
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadPortrait  => $"https://game.gtimg.cn/images/lol/act/img/champion/{Alias}.png";
        /// <summary>
        /// 标题名称：安妮
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 角色类型：mage、fighter、tank
        /// </summary>
        public List<string> Roles { get; set; } = new();
        /// <summary>
        /// 是否周免
        /// </summary>
        public int IsWeekFree { get; set; }
        /// <summary>
        /// 攻击能力值
        /// </summary>
        public int Attack { get; set; }
        /// <summary>
        /// 防御能力值
        /// </summary>
        public int Defense { get; set; }
        /// <summary>
        /// 魔法能力值
        /// </summary>
        public int Magic { get; set; }
        /// <summary>
        /// 上手难度值
        /// </summary>
        public int Difficulty { get; set; }
        /// <summary>
        /// 选中声音地址
        /// </summary>
        public string SelectAudio { get; set; } = string.Empty;
        /// <summary>
        /// 禁用声音地址
        /// </summary>
        public string BanAudio { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public int IsARAMWeekFree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsPermanentWeekFree { get; set; }
        /// <summary>
        /// 改动标签：改动英雄
        /// </summary>
        public string ChangeLabel { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public int GoldPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CouponPrice { get; set; }
        /// <summary>
        /// 阵营名称
        /// </summary>
        public string? Camp { get; set; }
        /// <summary>
        /// 阵营id
        /// </summary>
        public int? CampId { get; set; }
        /// <summary>
        /// 搜索词：安妮,黑暗之女,火女,Annie,anni,heianzhinv,huonv,an,hazn,hn
        /// </summary>
        public string KeyWords { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Instance_Id { get; set; } = string.Empty;

        /// <summary>
        /// 位置推荐值
        /// </summary>
        public HeroPositionRecommend PositionRecommend { get; set; } = new();
    }
}
