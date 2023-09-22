using CommunityToolkit.Mvvm.ComponentModel;

namespace LOL.Assist.Core.Models;

public class RuneListResponse
{
    /// <summary>
    /// 符文列表
    /// </summary>
    public Dictionary<int, RuneResponse> Rune { get; set; }
}

[ObservableObject]
public partial class RuneResponse
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图片地址
    /// </summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 样式名称
    /// </summary>
    public string StyleName { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>
    /// +10%攻击速度
    /// </summary>
    public string LongDesc { get; set; } = string.Empty;

    /// <summary>
    /// +10%攻击速度
    /// </summary>
    public string ShortDesc { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string SlotLabel { get; set; } = string.Empty;

    /// <summary>
    /// +10%攻击速度
    /// </summary>
    public string Tooltip { get; set; } = string.Empty;



    /// <summary>
    /// 分组信息
    /// </summary>
    public string GroupName { get; set; } = string.Empty;

    /// <summary>
    /// 是否选中
    /// </summary>
    [ObservableProperty]
    private bool _isChecked;

    [ObservableProperty]
    private double _opacity= 0.2;
    partial void OnIsCheckedChanged(bool value) => Opacity = value ? 1 : 0.2;
}