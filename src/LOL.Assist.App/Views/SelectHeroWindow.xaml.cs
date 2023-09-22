using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.App.ViewModels;
using System.Windows;
using LOL.Assist.App.Views.Controls;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using LOL.Assist.Core.Models;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace LOL.Assist.App.Views;

/// <summary>
/// SelectHeroPage.xaml 的交互逻辑
/// </summary>
public partial class SelectHeroWindow
{
    public int SelectHeroId { get; private set; }
    private readonly SelectHeroViewModel _selectHeroViewModel;
    public SelectHeroWindow()
    {
        _selectHeroViewModel = Ioc.Default.GetService<SelectHeroViewModel>()!;
        DataContext = _selectHeroViewModel;
        InitializeComponent();
        Loaded += This_Loaded;
    }
    private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        HeroControl heroControl = (HeroControl)sender;
        SelectHeroId = (int)heroControl.Tag;
        DialogResult = true;
    }
    private void This_Loaded(object sender, RoutedEventArgs e) => _selectHeroViewModel.LoadShowHeroes();

    protected override void OnClosed(EventArgs e)
    {
        _selectHeroViewModel.SelectPosition = null;
        base.OnClosed(e);
    }

    private void RadioButton_Click(object sender, RoutedEventArgs e)
    {
        RadioButton radioButton = (RadioButton)sender;
        radioButton.IsChecked = !((radioButton.IsChecked ?? false) && Math.Abs(radioButton.Opacity - 1d) == 0);
        radioButton.Opacity = radioButton.IsChecked!.Value ? 1 : 0.5;
        _selectHeroViewModel.SelectPosition = radioButton.IsChecked.Value ? radioButton.Tag.ToString() : null;

    }

    private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
    {
        RadioButton radioButton = (RadioButton)sender;
        radioButton.Opacity = 0.5;
    }
}