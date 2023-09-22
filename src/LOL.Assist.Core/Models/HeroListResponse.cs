namespace LOL.Assist.Core.Models;

public record HeroListResponse
{
    public List<Hero> Hero { get; set; } = new();
    /// <summary>
    /// 
    /// </summary>
    public string Version { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string FileTime { get; set; } = string.Empty;
}