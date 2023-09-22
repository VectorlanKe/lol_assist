namespace LOL.Assist.Core.Enums;

/// <summary>
/// 配置信息枚举
/// </summary>
[Flags]
public enum ConfigKeyEnum
{
    /// <summary>
    /// 自动接受
    /// </summary>
    AutoAccept,
    /// <summary>
    /// 自动禁用
    /// </summary>
    AutoBan,
    /// <summary>
    /// 自动选择
    /// </summary>
    AutoSelect,
    /// <summary>
    /// 自动随机禁用
    /// </summary>
    AutoBanRandom,
    /// <summary>
    /// 自动随机选择
    /// </summary>
    AutoSelectRandom,
    /// <summary>
    /// 非排位模式，自动优先选择对应位置英雄
    /// </summary>
    UnRankedSelectPosition,
    /// <summary>
    /// 自动符文天赋
    /// </summary>
    AutoPerk,
}