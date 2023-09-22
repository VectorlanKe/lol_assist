using LOL.Assist.Core.Models;
using Refit;

namespace LOL.Assist.Core.IServices;

/// <summary>
/// 基础数据服务
/// </summary>
public interface ILOLService
{
    /// <summary>
    /// 获取英雄列表
    /// </summary>
    /// <returns></returns>
    [Get("/act/lbp/common/guides/guideschampion_position.js")]
    Task<string> GetHeroPositionAsync();
}