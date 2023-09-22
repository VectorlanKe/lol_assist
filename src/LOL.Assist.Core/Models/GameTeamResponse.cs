namespace LOL.Assist.Core.Models;

public class GameTeamResponse
{
    /// <summary>
    /// 
    /// </summary>
    public int ChampionId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int LastSelectedSkinIndex { get; set; }
    /// <summary>
    /// 图标id
    /// </summary>
    public int ProfileIconId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Puuid { get; set; } = string.Empty;

    /// <summary>
    /// 上单：TOP
    /// 打野：JUNGLE
    /// 中单：MIDDLE
    /// 下路：BOTTOM
    /// 辅助：UTILITY
    /// </summary>
    public string SelectedPosition { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string SelectedRole { get; set; } = string.Empty;
    /// <summary>
    /// 召唤者id
    /// </summary>
    public long SummonerId { get; set; }
    /// <summary>
    /// 召唤者内部名称
    /// </summary>
    public string SummonerInternalName { get; set; } = string.Empty;
    /// <summary>
    /// 召唤者名称
    /// </summary>
    public string SummonerName { get; set; } = string.Empty;
    /// <summary>
    /// 是否组队
    /// </summary>
    public bool TeamOwner { get; set; }
    /// <summary>
    /// 组队id，相同则为组队
    /// </summary>
    public int TeamParticipantId { get; set; }
}