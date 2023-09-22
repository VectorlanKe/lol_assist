using LOL.Assist.Core.Models;
using Refit;

namespace LOL.Assist.Core.IServices;

/// <summary>
/// 基础数据服务
/// </summary>
public interface IGameService
{
    /// <summary>
    /// 获取英雄列表
    /// </summary>
    /// <returns></returns>
    [Get("/images/lol/act/img/js/heroList/hero_list.js")]
    Task<HeroListResponse> GetHeroListAsync();
    /// <summary>
    /// 获取符文列表
    /// </summary>
    /// <returns></returns>
    [Get("/images/lol/act/img/js/runeList/rune_list.js")]
    Task<RuneListResponse> GetRuneListAsync();
}