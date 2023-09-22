namespace LOL.Assist.Core.Models.SubscribeMessage;

public record ReadyCheckMessage
{
    /// <summary>
    /// 
    /// </summary>
    public List<string>? DeclinerIds { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? DodgeWarning { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PlayerResponse { get; set; }
    /// <summary>
    /// 状态信息
    /// InProgress 进行中,EveryoneReady
    /// </summary>
    public string? State { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? SuppressUx { get; set; }
    /// <summary>
    /// 记时0-15s
    /// </summary>
    public int Timer { get; set; }
}