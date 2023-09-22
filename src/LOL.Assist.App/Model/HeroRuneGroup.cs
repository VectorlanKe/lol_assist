using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Models;

namespace LOL.Assist.App.Model;

public partial class HeroRuneGroup:ObservableObject
{
    public HeroRune HeroRuneBase{get;set;}
    /// <summary>
    /// 主要系别符文信息
    /// </summary>
    public List<RuneResponse> Primary { get; set; }

    /// <summary>
    /// 副系及成长符文信息
    /// </summary>
    public List<RuneResponse> SecondGrowing { get; set; }
}