using LOL.Assist.App.ViewModels;
using System;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows.Controls;
using System.Linq;

namespace LOL.Assist.App.Views.Pages;

/// <summary>
/// RuneGenius.xaml 的交互逻辑
/// </summary>
public partial class RunePage
{
    private readonly RuneViewModel _runeViewModel;
    public RunePage()
    {
        InitializeComponent();
        _runeViewModel = Ioc.Default.GetService<RuneViewModel>()!;
        DataContext = _runeViewModel;
    }

    private void GetBack_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (Application.Current.MainWindow is not MainWindow maintain)
            return;
        maintain.FramePage.Source = new Uri("pages/SettingPage.xaml", UriKind.Relative);
    }

    private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {

    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        RunePopupWindow runePopupWindow = new RunePopupWindow()
        {
            Owner = Window.GetWindow(this)
        };
        runePopupWindow.ShowDialog();
        _runeViewModel.RefreshHeroRune();
    }
    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        Label button = (Label)sender;
        int championId = int.Parse(button.Tag.ToString() ?? "0");
        if (championId <= 0)
            return;
        _runeViewModel.RemoveHeroRune(championId);
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Update_MouseDown(object sender,RoutedEventArgs e)
    {
        Label grid = (Label)sender;
        int championId = int.Parse(grid.Tag?.ToString() ?? "0");
        if (championId <= 0)
            return;
        RunePopupWindow runePopupWindow = new RunePopupWindow()
        {
            Owner = Window.GetWindow(this)
        };
        runePopupWindow.SetSelectHeroRune(_runeViewModel.HeroRunes?.FirstOrDefault(p => p.HeroRuneBase.ChampionId == championId)?.HeroRuneBase);
        runePopupWindow.ShowDialog();
        _runeViewModel.RefreshHeroRune();
    }


}