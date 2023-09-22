using System.Collections.Generic;
using LOL.Assist.Core.Enums;
using LOL.Assist.Core.Models;
using LOL.Assist.Core.Models.SubscribeMessage;
using Newtonsoft.Json.Linq;

namespace LOL.Assist.App;

/// <summary>
/// 全局配置信息
/// </summary>
public static class Global
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public static List<Hero> Heroes { get; set; } = new List<Hero>();

    /// <summary>
    /// 位置信息
    /// </summary>
    public static List<Position> Positions = new List<Position>
    {
        new Position("上路","/Resources/Position_Challenger-Top.png","top"),
        new Position("打野","/Resources/Position_Challenger-Jungle.png","jungle"),
        new Position("中路","/Resources/Position_Challenger-Mid.png","middle"),
        new Position("下路","/Resources/Position_Challenger-Bot.png","bottom"),
        new Position("辅助","/Resources/Position_Challenger-Support.png","utility"),
    };

    /// <summary>
    /// 符文列表
    /// </summary>
    public static List<RuneResponse> Runes { get; set; } = new List<RuneResponse>();

    /// <summary>
    /// 当前游戏开始cell id
    /// </summary>
    public static int? LocalPlayerCellId { get; set; }
    /// <summary>
    /// 存储会话信息
    /// </summary>
    /// <param name="gameFlow"></param>
    public static void GameSessionHandle(JToken gameFlow, string uri, string type)
    {
        LocalPlayerCellId = gameFlow.ToObject<ChampSelectMessage>()?.LocalPlayerCellId;
    }

}