namespace LOL.Assist.Core.Models.SubscribeMessage;

/// <summary>
/// 禁用、选择响应信息
/// </summary>
public class ChampSelectMessage
{
    /// <summary>
    /// 0：ban、1：pick
    /// </summary>
    public List<List<ActionsItem>> Actions { get; set; }
    /// <summary>
    /// 位置序号
    /// </summary>
    public int LocalPlayerCellId { get; set; }
    /// <summary>
    /// 队伍信息
    /// </summary>
    public List<TeamInfo>? MyTeam{get; set; }
}
public class ActionsItem
{
    /// <summary>
    /// 位置序号
    /// </summary>
    public int ActorCellId { get; set; }
    /// <summary>
    /// 禁用或选用的英雄id
    /// </summary>
    public int ChampionId { get; set; }
    /// <summary>
    /// 是否完成操作
    /// </summary>
    public bool Completed { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsAllyAction { get; set; }
    /// <summary>
    /// 是否进行中
    /// </summary>
    public bool IsInProgress { get; set; }
    /// <summary>
    /// ban、pick
    /// </summary>
    public string Type { get; set; } = "ban";
}

/// <summary>
/// 队伍信息
/// </summary>
public class TeamInfo
{
    /// <summary>
    /// 位置信息
    /// top、jungle、middle、bottom、utility
    /// </summary>
    public string AssignedPosition { get; set; } = string.Empty;
    /// <summary>
    /// 位置序号
    /// </summary>
    public int CellId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ChampionId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ChampionPickIntent { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string EntitledFeatureType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string NameVisibilityType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ObfuscatedPuuid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ObfuscatedSummonerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Puuid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int SelectedSkinId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Spell1Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Spell2Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SummonerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Team { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int WardSkinId { get; set; }
}