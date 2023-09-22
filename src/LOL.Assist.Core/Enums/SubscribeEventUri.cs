using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOL.Assist.Core.Enums
{
    /// <summary>
    /// 用户事件url
    /// </summary>
    public static class SubscribeEventUri
    {
        /// <summary>
        /// 匹配到对局
        /// </summary>
        public const string GameReadyCheckTeam = "/lol-lobby-team-builder/v1/matchmaking";
        public const string GameSessionTeam = "/lol-lobby-team-builder/champ-select/v1/session";

        /// <summary>
        /// 寻找对局，包含对局状态信息
        /// </summary>
        public const string SearchGame = "/lol-matchmaking/v1/search";
        
        /// <summary>
        /// 对局会话信息
        /// </summary>
        public const string GameSession = "/lol-gameflow/v1/session";
        /// <summary>
        /// 对局阶段状态信息
        /// Matchmaking:排队
        /// ReadyCheck:待接受对局
        /// ChampSelect:对局准备
        /// InProgress:对局中
        /// WaitingForStats
        /// PreEndOfGame
        /// EndOfGame
        /// </summary>
        public const string GamePhase = "/lol-gameflow/v1/gameflow-phase";
        /// <summary>
        /// 对局校验（待确认接受或取消对局）
        /// </summary>
        public const string GameReadyCheck = "/lol-matchmaking/v1/ready-check";

        /// <summary>
        /// 对局会话信息
        /// </summary>
        public const string GameSessionChamp = "/lol-champ-select/v1/session";

        /// <summary>
        /// 玩家禁用或选择英雄通知
        /// /lol-champ-select/v1/summoners/{actorCellId}
        /// </summary>
        public const string SummonersChamp = "/lol-champ-select/v1/summoners";
        /// <summary>
        /// 禁用或选择的英雄
        /// /lol-champ-select/v1/grid-champions/{championId}
        /// </summary>
        public const string SelectChampion = "/lol-champ-select/v1/grid-champions";
        /// <summary>
        /// 聊天会话
        /// 聊天组：/lol-chat/v1/conversations/{id}
        /// 参与者：/lol-chat/v1/conversations/{id}/participants/{pId}
        /// 消息：/lol-chat/v1/conversations/{id}/messages/{mId}
        /// </summary>
        public const string ChatConversations = "/lol-chat/v1/conversations";

        /// <summary>
        /// 地图位置信息（红蓝），召唤师位置信息
        /// </summary>
        public const string MapSideSummoners = "/lol-champ-select/v1/pin-drop-notification";

    }
}
