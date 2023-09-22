namespace LOL.Assist.Core.Models.SubscribeMessage;

/// <summary>
/// 部分字段
/// </summary>
public class SelectSummonersMessage
{
    /// <summary>
    /// 召集id
    /// </summary>
    public int CellId { get; set; }
    /// <summary>
    /// 英雄id
    /// </summary>
    public int ChampionId { get; set; }
    /// <summary>
    /// 英雄名称
    /// </summary>
    public string ChampionName { get; set; }
    /// <summary>
    ///  "" /"pick"
    /// </summary>
    public string ActiveActionType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsActingNow { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsDonePicking { get; set; }
}