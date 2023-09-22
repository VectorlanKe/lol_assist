using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.App.ViewModels;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Enums;
using LOL.Assist.Core.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LOL.Assist.Core.Models;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using LOL.Assist.Core;
using Newtonsoft.Json.Linq;

namespace LOL.Assist.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        private readonly IThemeService _themeService;
        private bool _initialized;

        public MainWindow()
        {
            InitializeComponent();

            _themeService = Ioc.Default.GetService<IThemeService>()!;
            SetThemeLabel();
            Loaded += (_, _) => Initialize();
            LolConnect.ConnectNotificationAction += SetWindowLocation;
        }

        private void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;
            Loading.Visibility = Visibility.Visible;
            FramePage.Visibility = Visibility.Hidden;
            Task.Run(async () =>
            {
                IDbService dbService = Ioc.Default.GetService<IDbService>()!;
                IGameService gameService = Ioc.Default.GetService<IGameService>()!;
                ILOLService lolService = Ioc.Default.GetService<ILOLService>()!;
                //加载默认监听事件
                LolConnect.ManageEventAction(true, SubscribeEventUri.GameSessionChamp, Global.GameSessionHandle!);
                //加载全局数据
                Global.Heroes = (await gameService.GetHeroListAsync()).Hero;
                string heroPosition = await lolService.GetHeroPositionAsync();
                if (!string.IsNullOrWhiteSpace(heroPosition))
                {
                    string heroPositionJson = heroPosition.Split('=', ';')[1];
                    Dictionary<int, HeroPositionRecommend>? heroPositionRecommend = JObject.Parse(heroPositionJson)["list"]?.ToObject<Dictionary<int, HeroPositionRecommend>>();
                    if (heroPositionRecommend != null)
                        Global.Heroes.ForEach(p => p.PositionRecommend = heroPositionRecommend.GetValueOrDefault(p.HeroId) ?? new HeroPositionRecommend());
                }
                Global.Runes = (await gameService.GetRuneListAsync()).Rune.Select(p =>
                {
                    p.Value.Id = p.Key;
                    return p.Value;
                }).ToList();

                //加载配置内容
                SettingViewModel settingsViewModel = Ioc.Default.GetService<SettingViewModel>()!;
                IList<Config> configs = dbService.GetAll<Config>();
                foreach (var config in configs)
                {
                    switch (config.Key)
                    {
                        case ConfigKeyEnum.AutoAccept:
                            settingsViewModel.AutoAccept = bool.Parse(config.Value);
                            settingsViewModel.AutoAcceptDelayTime = config.DelayTime;
                            break;
                        case ConfigKeyEnum.AutoBan:
                            settingsViewModel.AutoAcceptBan = bool.Parse(config.Value);
                            settingsViewModel.AutoAcceptBanDelayTime = config.DelayTime;
                            IList<SelectHero> selectSessionBanHeroes = dbService.GetAll<SelectHero>(p => p.Type == ConfigKeyEnum.AutoBan);
                            foreach (SelectHero selectHero in selectSessionBanHeroes.OrderBy(p => p.Priority))
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    settingsViewModel.AutoAcceptBanHeroes[selectHero.Priority] = selectHero;
                                });
                            }
                            break;
                        case ConfigKeyEnum.AutoSelect:
                            settingsViewModel.AutoAcceptSelect = bool.Parse(config.Value);
                            settingsViewModel.AutoAcceptSelectDelayTime = config.DelayTime;
                            IList<SelectHero> selectSessionSelectHeroes = dbService.GetAll<SelectHero>(p => p.Type == ConfigKeyEnum.AutoSelect);
                            foreach (var selectHeroGroup in selectSessionSelectHeroes.GroupBy(p => p.Portrait))
                            {
                                SelectHeroGroup? heroGroup = settingsViewModel.AutoAcceptSelectHeroGroups
                                    .FirstOrDefault(p => p.HeroPosition.Portrait == selectHeroGroup.Key);
                                if (heroGroup == null)
                                    continue;
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    selectHeroGroup.ToList().ForEach(selectHero =>
                                        heroGroup.SelectHeroes[selectHero.Priority] = selectHero);
                                });

                            }
                            break;
                        case ConfigKeyEnum.AutoBanRandom:
                            settingsViewModel.AutoAcceptBanRandom = bool.Parse(config.Value);
                            break;
                        case ConfigKeyEnum.AutoSelectRandom:
                            settingsViewModel.AutoAcceptSelectRandom = bool.Parse(config.Value);
                            break;
                        case ConfigKeyEnum.UnRankedSelectPosition:
                            settingsViewModel.UnRankedSelectPosition = int.Parse(config.Value);
                            break;
                        case ConfigKeyEnum.AutoPerk:
                            settingsViewModel.AutoPerk = bool.Parse(config.Value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                await Dispatcher.InvokeAsync(() =>
                {
                    Loading.Visibility = Visibility.Hidden;
                    FramePage.Visibility = Visibility.Visible;
                });

                return true;
            });
        }

        /// <summary>
        /// 设置窗口位置
        /// </summary>
        private void SetWindowLocation()
        {
            IWindowService windowService = Ioc.Default.GetService<IWindowService>()!;
            var rect = windowService.GetWindowsRectLocation();
            if (rect == null)
                return;
            Dispatcher.Invoke(() =>
            {
                Left = rect.Value.Left - Width;
                Top = rect.Value.Top;
            });
        }
        private void NotifyBarExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            NotifyMenuBar = null;
        }

        private void ButtonTheme_OnClick(object sender, RoutedEventArgs e)
        {
            _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
            SetThemeLabel();
        }

        private void SetThemeLabel(ThemeType? themeType = null)
        {
            themeType ??= _themeService.GetTheme();
            // ThemeLabel.Content = themeType == ThemeType.Dark ? "黑暗" : "明亮";
        }
    }
}
