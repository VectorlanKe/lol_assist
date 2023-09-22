namespace LOL.Assist.Core.Models;

public class ConversationsResponse
{
    /// <summary>
    /// 
    /// </summary>
    public string GameName { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string GameTag { get; set; } = string.Empty;
    /// <summary>
    /// 聊天id
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string InviterId { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string IsMuted { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public LastMessage? LastMessage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public MucJwtDto? MucJwtDto { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Pid { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string TargetRegion { get; set; } = string.Empty;
    /// <summary>
    /// chat
    /// </summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public int UnreadMessageCount { get; set; }
}
public class LastMessage
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Body { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string FromId { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public int FromObfuscatedSummonerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string FromPid { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public int FromSummonerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string IsHistorical { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Timestamp { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; } = string.Empty;
}

public class MucJwtDto
{
    /// <summary>
    /// 
    /// </summary>
    public string ChannelClaim { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Domain { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Jwt { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string TargetRegion { get; set; } = string.Empty;
}