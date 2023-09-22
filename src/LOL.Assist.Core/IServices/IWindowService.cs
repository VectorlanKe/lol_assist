using LOL.Assist.Core.Services;

namespace LOL.Assist.Core.IServices;

public interface IWindowService
{
    /// <summary>
    /// 默认进程名称
    /// </summary>
    public const string LolProcessName = "League of Legends";
    /// <summary>
    /// 设置窗体位置
    /// </summary>
    WindowService.GetWindowsProcess.Rect? GetWindowsRectLocation(string name = LolProcessName);

    /// <summary>
    /// 设置窗体大小
    /// </summary>
    bool SetWindowsSize(int x, int y, string name = LolProcessName);
}