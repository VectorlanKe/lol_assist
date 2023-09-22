using CommunityToolkit.Mvvm.DependencyInjection;
using FreeSql;
using LOL.Assist.App.ViewModels;
using LOL.Assist.App.Views;
using LOL.Assist.Core;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Enums;
using LOL.Assist.Core.IServices;
using LOL.Assist.Core.Models;
using LOL.Assist.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace LOL.Assist.App
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static LolConnect _lolConnect=new LolConnect();
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection()
                .AddSingleton<IThemeService, ThemeService>();

            services.AddSingleton(RestService.For<ILcuService>(_lolConnect.GetHttpClient()));
            RefitSettings settings = new RefitSettings(new NewtonsoftJsonContentSerializer());
            services.AddSingleton(RestService.For<ILOLService>("https://lol.qq.com", settings));
            services.AddSingleton(RestService.For<IGameService>("https://game.gtimg.cn", settings));
            services.AddSingleton(new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, $"Data Source={AppContext.BaseDirectory}config.db")
                .UseAutoSyncStructure(true).Build());
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<IWindowService, WindowService>();

            services.AddScoped<SettingViewModel>();
            services.AddScoped<SelectHeroViewModel>();
            services.AddScoped<RuneViewModel>();
            services.AddScoped<RunePopupViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
            base.OnStartup(e);
        }
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            _lolConnect=_lolConnect.Connect();
        }

    }

}
