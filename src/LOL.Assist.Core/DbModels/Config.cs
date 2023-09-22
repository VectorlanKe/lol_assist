using System.ComponentModel.DataAnnotations;
using LOL.Assist.Core.Enums;

namespace LOL.Assist.Core.DbModels;

/// <summary>
/// 配置信息
/// </summary>
public class Config
{
    /// <summary>
    /// 配置
    /// </summary>
    [Key]
    public ConfigKeyEnum Key { get; set; }
    /// <summary>
    /// 配置值
    /// </summary>
    [MaxLength(50)]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// 延迟时间
    /// </summary>
    public int DelayTime { get; set; }
}