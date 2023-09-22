using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.App.ViewModels;
using System.Linq;
using System.Windows;
using LOL.Assist.App.Views.Controls;
using LOL.Assist.Core.Models;
using LOL.Assist.Core.DbModels;
using System;

namespace LOL.Assist.App.Views.Pages;

/// <summary>
/// Settings.xaml 的交互逻辑
/// </summary>
public partial class SettingPage
{
    private readonly SettingViewModel _settingsViewModel;
    public SettingPage()
    {
        InitializeComponent();
        _settingsViewModel = Ioc.Default.GetService<SettingViewModel>()!;
        DataContext = _settingsViewModel;
    }
    private void HeroControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        HeroControl heroControl = (HeroControl)sender;
        SelectHeroWindow selectHeroWindow = new SelectHeroWindow
        {
            Owner = Window.GetWindow(this)
        };
        selectHeroWindow.InitializeComponent();
        selectHeroWindow.ShowDialog();
        Hero? hero = Global.Heroes.FirstOrDefault(p => p.HeroId == selectHeroWindow.SelectHeroId);
        if (hero == null)
            return;
        SelectHero selectHero = new SelectHero()
        {
            Name = hero.Name,
            HeadPortrait = hero.HeadPortrait,
            Priority = heroControl.Priority,
            ChampionId = hero.HeroId,
            Portrait = heroControl.Position,
        };
        switch (heroControl.Tag)
        {
            case "Ban":
                selectHero.Type = Core.Enums.ConfigKeyEnum.AutoBan;
                _settingsViewModel.AutoAcceptBanHeroes[heroControl.Priority] = selectHero;
                break;
            case "Select":
                selectHero.Type = Core.Enums.ConfigKeyEnum.AutoSelect;
                _settingsViewModel.AutoAcceptSelectHeroGroups
                    .FirstOrDefault(p => p.HeroPosition.Portrait == selectHero.Portrait)!
                    .SelectHeroes[heroControl.Priority] = selectHero;
                break;
        }

    }
    /// <summary>
    /// 符文页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RuneButton_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is not MainWindow maintain)
            return;
        maintain.FramePage.Source = new Uri("pages/RunePage.xaml", UriKind.Relative);
    }
}