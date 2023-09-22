using System.Collections.Generic;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using LOL.Assist.Core.Models;

namespace LOL.Assist.App.Model;
/// <summary>
/// 符文分组信息
/// </summary>
public partial class RuneGroup:ObservableObject
{
    /// <summary>
    /// 主系选中状态
    /// </summary>
    [ObservableProperty]
    private double _primaryOpacity = 0.4;
    /// <summary>
    /// 副系显隐藏
    /// Visible
    /// Hidden
    /// Collapsed
    /// </summary>
    [ObservableProperty] 
    private Visibility _secondVisibility;
    /// <summary>
    /// 副系选中状态
    /// </summary>
    [ObservableProperty]
    private double _secondOpacity = 0.4;
    /// <summary>
    /// 系别
    /// </summary>
    [ObservableProperty] private RuneResponse _root;
    /// <summary>
    /// 基石信息
    /// </summary>
    public List<RuneResponse> Base { get; set; }

    /// <summary>
    /// 一级符文信息
    /// </summary>
    public List<RuneResponse> Valiant { get; set; }
    /// <summary>
    /// 二级符文信息
    /// </summary>
    public List<RuneResponse> Legend { get; set; }
    /// <summary>
    /// 三级符文信息
    /// </summary>
    public List<RuneResponse> Combat { get; set; }
}
