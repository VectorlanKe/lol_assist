using System.ComponentModel.DataAnnotations;

namespace LOL.Assist.Core.DbModels
{
    /// <summary>
    /// 符文列表
    /// </summary>
    public class HeroRune
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Key]
        public int ChampionId { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        [MaxLength(200)]
        public string HeadPortrait { get; set; } = string.Empty;
        /// <summary>
        /// 名称： 黑暗之女
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 搜索词
        /// </summary>
        [MaxLength(200)]
        public string KeyWords { get; set; } = string.Empty;
        /// <summary>
        /// 符文信息
        /// </summary>
        [MaxLength(200)]
        public string RuneJson { get; set; } = string.Empty;
    }
}
