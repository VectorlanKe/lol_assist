using System.ComponentModel.DataAnnotations;
using LOL.Assist.Core.Enums;

namespace LOL.Assist.Core.DbModels;

public class SelectHero
{
    /// <summary>
    /// 优先级
    /// </summary>
    [Key]
    public int Priority { get; set; }
    /// <summary>
    /// 位置信息
    /// </summary>
    [Key,MaxLength(10)]
    public string Portrait{get;set;}=string.Empty;
    /// <summary>
    /// ban、pick
    /// </summary>
    [Key, MaxLength(5)]
    public ConfigKeyEnum Type { get; set; } = ConfigKeyEnum.AutoBan;
    /// <summary>
    /// 角色id
    /// </summary>
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

    
    
}