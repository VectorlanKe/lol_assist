using LOL.Assist.Core.Models;
using LOL.Assist.Core.Models.SubscribeMessage;
using Refit;

namespace LOL.Assist.Core.IServices;

public interface ILcuService
{
    /// <summary>
    /// 接受对局
    /// </summary>
    /// <returns></returns>
    [Post("/lol-matchmaking/v1/ready-check/accept")]
    Task<ApiResponse<string>> TeamBuilderReadyCheckAsync();
    /// <summary>
    /// 查询游戏会话
    /// </summary>
    /// <returns></returns>
    [Get("/lol-gameflow/v1/session")]
    Task<ApiResponse<string>> GetGameFlowSessionAsync();

    //lol-lobby/v1/lobby/custom/cancel-champ-select
    /// <summary>
    /// 获取选人会话
    /// </summary>
    /// <returns></returns>
    [Get("/lol-champ-select/v1/session")]
    Task<ApiResponse<ChampSelectMessage>> GetChampSessionAsync();

    /// <summary>
    /// 选择禁用英雄
    /// </summary>
    /// <param name="actionId">当前用户action id</param>
    /// <param name="body"></param>
    /// <returns></returns>
    [Patch("/lol-champ-select/v1/session/actions/{actionId}")]
    Task<ApiResponse<string>> ChampSelectSessionActionsAsync(int actionId, [Body] SessionActionsRequest body);



    /// <summary>
    /// 获取当前符文页
    /// </summary>
    /// <returns></returns>
    [Get("/lol-perks/v1/currentpage")]
    Task<ApiResponse<PerkCurrentPageResponse>> GetPerkCurrentPageAsync();
    /// <summary>
    /// 删除符文页
    /// </summary>
    /// <returns></returns>
    [Delete("/lol-perks/v1/pages/{id}")]
    Task<ApiResponse<string>> DeletePerkPageAsync(long id);
    /// <summary>
    /// 修改符文页
    /// </summary>
    /// <returns></returns>
    [Put("/lol-perks/v1/pages/{id}")]
    Task<ApiResponse<string>> UpdatePerkPageAsync(long id,[Body] PerkPageRequest body);
    /// <summary>
    /// 创建符文页
    /// </summary>
    /// <returns></returns>
    [Post("/lol-perks/v1/pages")]
    Task<ApiResponse<string>> CreatePerkPageAsync([Body] PerkPageRequest body);



    /// <summary>
    /// 获取的是整个房间的数据，包含了观众、机器人。
    /// </summary>
    /// <returns></returns>
    [Get("/lol-lobby/v2/lobby")]
    Task<ApiResponse<string>> GetRoomAsync();
    /// <summary>
    /// 获取的是队伍真实玩家数据，观众、机器人不在列表中。
    /// </summary>
    /// <returns></returns>
    [Get("/lol-lobby/v2/lobby/members")]
    Task<ApiResponse<string>> GetRoomMembersAsync();

    /// <summary>
    /// 获取当前聊天消息组信息
    /// </summary>
    /// <returns></returns>
    [Get("/lol-chat/v1/conversations")]
    Task<ApiResponse<List<ConversationsResponse>>> GetConversationsAsync();
    /// <summary>
    /// 获取当前聊天组消息
    /// </summary>
    /// <param name="conversationId"></param>
    /// <returns></returns>
    [Get("/lol-chat/v1/conversations/{conversationId}/messages")]
    Task<ApiResponse<string>> GetConversationsMessageAsync(string conversationId);
    /// <summary>
    /// 发送消息到聊天组
    /// </summary>
    /// <param name="conversationId"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    [Post("/lol-chat/v1/conversations/{conversationId}/messages")]
    Task<ApiResponse<string>> SendConversationsMessageAsync(string conversationId, [Body] ConversationsMsgRequest body);


    /// <summary>
    /// 设置英雄皮肤及召唤师技能
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [Patch("/lol-champ-select/v1/session/my-selection")]
    Task<ApiResponse<string>> ChampSelectSession([Body] ChampSelectSessionRequest body);

    /// <summary>
    /// 获取jwt
    /// </summary>
    /// <returns></returns>
    [Get("/lol-summoner/v1/current-summoner/jwt")]
    Task<ApiResponse<string>> GetCurrentSummonerJwt();
    /// <summary>
    /// 设置头像
    /// </summary>
    /// <returns></returns>
    [Put("/lol-summoner/v1/current-summoner/icon")]
    Task<ApiResponse<string>> CurrentSummonerIcon([Body]SummonerIconRequest body);
    
}