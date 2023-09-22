using CommunityToolkit.Mvvm.ComponentModel;
using LOL.Assist.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LOL.Assist.App.ViewModels;

public partial class SelectHeroViewModel : ObservableObject
{

    [ObservableProperty] private string _searchText = string.Empty;

    [ObservableProperty] private int _showHeroColumns = 6;

    [ObservableProperty] private List<Hero> _showHeroes = new();

    [ObservableProperty] private int _showHeroRows;

    [ObservableProperty] private string? _selectPosition = string.Empty;


    [ObservableProperty] private Visibility _showLoading = Visibility.Visible;


    /// <summary>
    /// 位置信息
    /// </summary>
    [ObservableProperty]
    private List<Position> _positions = Global.Positions;


    partial void OnSearchTextChanged(string value) => LoadShowHeroes();
    partial void OnSelectPositionChanged(string value) => LoadShowHeroes();
    public void LoadShowHeroes()
    {
        ShowLoading = Visibility.Visible;
        IEnumerable<Hero> positionRecommend = SelectPosition switch
        {
            "top" => Global.Heroes.Where(p => p.PositionRecommend.Top >= 100).OrderByDescending(px => px.PositionRecommend.Top),
            "jungle" => Global.Heroes.Where(p => p.PositionRecommend.Jungle >= 100).OrderByDescending(px => px.PositionRecommend.Jungle),
            "middle" => Global.Heroes.Where(p => p.PositionRecommend.Mid >= 100).OrderByDescending(px => px.PositionRecommend.Mid),
            "bottom" => Global.Heroes.Where(p => p.PositionRecommend.Bottom >= 100).OrderByDescending(px => px.PositionRecommend.Bottom),
            "utility" => Global.Heroes.Where(p => p.PositionRecommend.Support >= 100).OrderByDescending(px => px.PositionRecommend.Support),
            _ => Global.Heroes,
        };
        ShowHeroes = (string.IsNullOrWhiteSpace(SearchText) ?
                positionRecommend :
                positionRecommend.Where(p => p.Name.Contains(SearchText) || p.KeyWords.Contains(SearchText)))
            .ToList();
        var rows = (int)Math.Ceiling(1d * ShowHeroes.Count / ShowHeroColumns);
        ShowHeroRows = rows <= 5 ? 5 : rows;
        ShowLoading = Visibility.Collapsed;
    }
}