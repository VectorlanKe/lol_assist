namespace LOL.Assist.Core.Models;

/// <summary>
/// 
/// </summary>
/// <param name="ChampionId"></param>
/// <param name="Type">ban、pick</param>
/// <param name="Completed"></param>
public record SessionActionsRequest(int ChampionId, string Type, bool Completed = true,bool IsAllyAction=true);