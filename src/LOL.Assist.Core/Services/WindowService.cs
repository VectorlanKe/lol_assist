using LOL.Assist.Core.IServices;
using System.Runtime.InteropServices;

namespace LOL.Assist.Core.Services;


public class WindowService : IWindowService
{
    
    /// <summary>
    /// 设置窗体位置
    /// </summary>
    public GetWindowsProcess.Rect? GetWindowsRectLocation(string name = IWindowService.LolProcessName)
    {
        IntPtr windowPtr = GetWindowsProcess.FindWindow(null, name);
        GetWindowsProcess.Rect result = new GetWindowsProcess.Rect();
        GetWindowsProcess.GetWindowRect(windowPtr, ref result);
        if (result.Top < 0 || result.Left < 0)
            return null;
        return result;
    }
    /// <summary>
    /// 设置窗体大小
    /// </summary>
    public bool SetWindowsSize(int x, int y, string name = IWindowService.LolProcessName)
    {
        IntPtr windowPtr = GetWindowsProcess.FindWindow(null, name);
        bool result = GetWindowsProcess.SetWindowPos(windowPtr, -1, 0, 0, x, y, 0x0002 | 0x0200);
        return result;
    }
    /// <summary>
    /// 参考文档
    /// https://learn.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindow
    /// https://cloud.tencent.com/developer/article/1088763
    /// </summary>
    public static class GetWindowsProcess
    {
        /// <summary>
        /// 获取所有窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr GetWindow();
        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <param name="lpClassName">窗体的类名称，比如Form、Window。若不知道，指定为null即可</param>
        /// <param name="lpWindowName">窗体的标题/文字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowA", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 设置窗口位置及大小
        /// https://learn.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setwindowpos
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);
        /// <summary>
        /// 获取窗口大小及位置信息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            /// <summary>
            /// 最左坐标
            /// </summary>
            public int Left;
            /// <summary>
            /// 最上坐标
            /// </summary>
            public int Top;
            /// <summary>
            /// 最右坐标
            /// </summary>
            public int Right;
            /// <summary>
            /// 最下坐标
            /// </summary>
            public int Bottom;
        }
    }
}
