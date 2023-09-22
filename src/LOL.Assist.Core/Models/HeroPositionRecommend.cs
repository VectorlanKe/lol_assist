namespace LOL.Assist.Core.Models;

/// <summary>
/// 英雄位置推荐
/// </summary>
public class HeroPositionRecommend
{
    /// <summary>
    /// 上单推荐值
    /// </summary>
    public int Top { get; set; }
    /// <summary>
    /// 打野推荐值
    /// </summary>
    public int Jungle { get; set; }
    /// <summary>
    /// 中单推荐值
    /// </summary>
    public int Mid { get; set; }
    /// <summary>
    /// 下路推荐值
    /// </summary>
    public int Bottom { get; set; }
    /// <summary>
    /// 辅助推荐值
    /// </summary>
    public int Support { get; set; }
}