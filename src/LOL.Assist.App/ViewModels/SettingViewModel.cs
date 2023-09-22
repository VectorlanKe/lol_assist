using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.Core;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Enums;
using LOL.Assist.Core.IServices;
using LOL.Assist.Core.Models;
using LOL.Assist.Core.Models.SubscribeMessage;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LOL.Assist.App.ViewModels;

public partial class SettingViewModel : ObservableObject
{
    /// <summary>
    /// 自动接受
    /// </summary>
    [ObservableProperty]
    private bool _autoAccept;
    /// <summary>
    /// 自动接受联动组件显隐
    /// </summary>
    [ObservableProperty]
    private Visibility _autoAcceptVisibility = Visibility.Collapsed;
    /// <summary>
    /// 自动接受延迟时间
    /// </summary>
    [ObservableProperty]
    private int _autoAcceptDelayTime;

    /// <summary>
    /// 自动禁用
    /// </summary>
    [ObservableProperty]
    private bool _autoAcceptBan;
    /// <summary>
    /// 自动禁用联动组件显隐
    /// </summary>
    [ObservableProperty]
    private Visibility _autoAcceptBanHeroVisibility = Visibility.Collapsed;
    /// <summary>
    /// 自动禁用延迟时间
    /// </summary>
    [ObservableProperty]
    private int _autoAcceptBanDelayTime;
    /// <summary>
    /// 自动根据列表随机禁用
    /// </summary>
    [ObservableProperty]
    private bool _autoAcceptBanRandom;
    /// <summary>
    /// 自动禁用英雄列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<SelectHero> _autoAcceptBanHeroes = new(Enumerable.Range(0, 5).Select(i => new SelectHero
    {
        ChampionId = -1,
        HeadPortrait = _positions[i].Image,
        Name = "无",
        Priority = i,
    }));

    /// <summary>
    /// 自动配置符文天赋
    /// </summary>
    [ObservableProperty]
    private bool _autoPerk;

    /// <summary>
    /// 自动选择
    /// </summary>
    [ObservableProperty]
    private bool _autoAcceptSelect;
    /// <summary>
    /// 自动选择联动组件显隐
    /// </summary>
    [ObservableProperty]
    private Visibility _autoAcceptSelectHeroVisibility = Visibility.Hidden;

    /// <summary>
    /// 自动选择延迟时间
    /// </summary>
    [ObservableProperty] private int _autoAcceptSelectDelayTime;
    /// <summary>
    /// 自动根据列表随机选用
    /// </summary>
    [ObservableProperty]
    private bool _autoAcceptSelectRandom;


    /// <summary>
    /// 非排位模式，自动优先选择对应位置英雄
    /// </summary>
    [ObservableProperty]
    private int _unRankedSelectPosition;

    /// <summary>
    /// 位置信息
    /// </summary>
    [ObservableProperty]
    private static List<Position> _positions = Global.Positions;

    /// <summary>
    /// 自动选用英雄列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<SelectHeroGroup> _autoAcceptSelectHeroGroups = new(_positions.Select(
        p => new SelectHeroGroup
        {
            HeroPosition = p,
            SelectHeroes = new ObservableCollection<SelectHero>(Enumerable.Range(0, 3).Select(i => new SelectHero
            {
                ChampionId = -1,
                HeadPortrait = p.Image,
                Name = "无",
                Priority = i,
                Portrait = p.Portrait,
                Type = ConfigKeyEnum.AutoSelect,
            }))
        }));
    /// <summary>
    /// 设置头像或背景id值
    /// </summary>
    [ObservableProperty]
    private string _profileIconId;

    /// <summary>
    /// 自动禁用或选用英雄状态值
    /// 0、1：禁用中、2：选用中
    /// </summary>
    private static int _autoHeroState;

    private static readonly object AutoHeroLock = new();

    private readonly ILcuService _lcuService;
    private readonly IDbService _dbService;
    private readonly IWindowService _windowService;
    public SettingViewModel()
    {
        _lcuService = Ioc.Default.GetService<ILcuService>()!;
        _dbService = Ioc.Default.GetService<IDbService>()!;
        _windowService = Ioc.Default.GetService<IWindowService>()!;
        _autoAcceptBanHeroes.CollectionChanged += SaveSelectHeroesChanged!;
        foreach (var selectHeroGroup in _autoAcceptSelectHeroGroups)
        {
            selectHeroGroup.SelectHeroes.CollectionChanged += SaveSelectHeroesChanged!;
        }
    }


    private void SessionActionBing(bool value, ConfigKeyEnum configKey)
    {

        int delayTime = 0;
        Visibility visibility = value ? Visibility.Visible : Visibility.Collapsed;
        switch (configKey)
        {
            case ConfigKeyEnum.AutoAccept:
                delayTime = AutoAcceptDelayTime;
                AutoAcceptVisibility = visibility;
                LolConnect.ManageEventAction(value, SubscribeEventUri.GameReadyCheck, AutoAcceptHandle!);
                break;
            case ConfigKeyEnum.AutoBan:
                delayTime = AutoAcceptBanDelayTime;
                AutoAcceptBanHeroVisibility = visibility;
                LolConnect.ManageEventAction(value, SubscribeEventUri.GameSessionChamp, AutoAcceptBanHandleAsync!);
                break;
            case ConfigKeyEnum.AutoSelect:
                delayTime = AutoAcceptSelectDelayTime;
                AutoAcceptSelectHeroVisibility = visibility;
                LolConnect.ManageEventAction(value, SubscribeEventUri.GameSessionChamp, AutoAcceptSelectHandleAsync!);
                break;
            case ConfigKeyEnum.AutoPerk:
                LolConnect.ManageEventAction(value, SubscribeEventUri.SummonersChamp, AutoPerkSetHandleAsync!);
                break;
            case ConfigKeyEnum.AutoBanRandom:
            case ConfigKeyEnum.AutoSelectRandom:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(configKey), configKey, null);
        }
        _dbService.InsertOrUpdate(new Config
        {
            Key = configKey,
            Value = value.ToString(),
            DelayTime = delayTime
        });
    }


    #region 自动接受对局

    partial void OnAutoAcceptChanged(bool value) => SessionActionBing(value, ConfigKeyEnum.AutoAccept);

    partial void OnAutoAcceptDelayTimeChanged(int value) => _dbService.UpdateConfigDelayTime(ConfigKeyEnum.AutoAccept, value);

    private void AutoAcceptHandle(JToken gameFlow, string uri, string type)
    {
        ReadyCheckMessage? message = gameFlow.ToObject<ReadyCheckMessage>();
        if (message == null)
            return;
        if (string.Compare(message.PlayerResponse, "None", StringComparison.OrdinalIgnoreCase) != 0)
            return;
        if (!AutoAccept || message.Timer != AutoAcceptDelayTime)
            return;
        _ = _lcuService.TeamBuilderReadyCheckAsync().Result;

        //重置选择状态
        _autoHeroState = 0;
    }
    #endregion

    #region 自动禁用或选择英雄
    /// <summary>
    /// 自动禁用英雄
    /// </summary>
    /// <param name="value"></param>
    partial void OnAutoAcceptBanChanged(bool value) => SessionActionBing(value, ConfigKeyEnum.AutoBan);
    partial void OnAutoAcceptBanDelayTimeChanged(int value) => _dbService.UpdateConfigDelayTime(ConfigKeyEnum.AutoBan, value);
    partial void OnAutoAcceptBanRandomChanged(bool value) => SessionActionBing(value, ConfigKeyEnum.AutoBanRandom);
    /// <summary>
    /// 禁用英雄处理
    /// </summary>
    /// <param name="gameFlow"></param>
    private void AutoAcceptBanHandleAsync(JToken gameFlow, string uri, string type) => AutoAcceptHeroesHandleAsync(gameFlow, ConfigKeyEnum.AutoBan);

    /// <summary>
    /// 自动配置天赋符文
    /// </summary>
    /// <param name="value"></param>
    partial void OnAutoPerkChanged(bool value) => SessionActionBing(value, ConfigKeyEnum.AutoPerk);

    /// <summary>
    /// 自动选择英雄
    /// </summary>
    /// <param name="value"></param>
    partial void OnAutoAcceptSelectChanged(bool value) => SessionActionBing(value, ConfigKeyEnum.AutoSelect);
    partial void OnAutoAcceptSelectDelayTimeChanged(int value) => _dbService.UpdateConfigDelayTime(ConfigKeyEnum.AutoSelect, value);
    partial void OnAutoAcceptSelectRandomChanged(bool value) => SessionActionBing(value, ConfigKeyEnum.AutoSelectRandom);
    /// <summary>
    /// 非排位模式，位置设置
    /// </summary>
    /// <param name="value"></param>
    partial void OnUnRankedSelectPositionChanged(int value)
    {
        _dbService.InsertOrUpdate(new Config
        {
            Key = ConfigKeyEnum.UnRankedSelectPosition,
            Value = value.ToString(),
        });
    }
    /// <summary>
    /// 选用英雄处理
    /// </summary>
    /// <param name="gameFlow"></param>
    private void AutoAcceptSelectHandleAsync(JToken gameFlow, string uri, string type) => AutoAcceptHeroesHandleAsync(gameFlow, ConfigKeyEnum.AutoSelect);


    /// <summary>
    /// 保存英雄选择信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="notifyCollectionChangedEventArgs"></param>
    private void SaveSelectHeroesChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        SelectHero newValue = notifyCollectionChangedEventArgs.NewItems!.OfType<SelectHero>().First();
        _dbService.InsertOrUpdate(newValue);
    }

    /// <summary>
    /// 自动禁用或选择英雄回调处理
    /// </summary>
    /// <param name="gameFlow"></param>
    /// <param name="configKey"></param>
    private async ValueTask AutoAcceptHeroesHandleAsync(JToken gameFlow, ConfigKeyEnum configKey)
    {
        //NORMAL 匹配
        //RANKED_SOLO_5x5  单双排
        //RANKED_FLEX_SR  组排
        //ARAM_UNRANKED_5x5  大乱斗5v5
        //URF 无限火力
        //BOT 人机
        //PRACTICETOOL  自定义
        ChampSelectMessage? message = gameFlow.ToObject<ChampSelectMessage>();
        if (message == null)
            return;
        await AutoAcceptBanPackHeroAsync(message, configKey);
    }
    /// <summary>
    /// 自动禁用或选择英雄
    /// 预选：15s,禁用、选用：30s
    /// </summary>
    /// <param name="champ"></param>
    /// <param name="sessionType"></param>
    /// <returns></returns>
    private async ValueTask AutoAcceptBanPackHeroAsync(ChampSelectMessage champ, ConfigKeyEnum sessionType)
    {
        //当前会话类型
        //NORMAL
        //Ban：禁用
        //Pick：选择
        if (champ?.Actions == null)
            return;
        string type = sessionType == ConfigKeyEnum.AutoBan ? "ban" : "pick";
        ActionsItem? action = champ.Actions
            .SelectMany(p => p).Where(p => p.ActorCellId == champ.LocalPlayerCellId)
            .FirstOrDefault(p => p.Type == type);
        if (action is not { IsInProgress: true })
            return;
        int sessionTypeInt = (int)sessionType;
        IEnumerable<SelectHero> selectHeroes = _dbService.GetAll<SelectHero>(p => p.Type == sessionType)
            .Where(p => p.ChampionId >= 0)
            .ToList();
        if (sessionTypeInt == _autoHeroState || !selectHeroes.Any())
            return;
        //是否随机禁用列表英雄
        bool isRandom;
        //延迟选择时间，秒
        int delayTime;
        switch (sessionType)
        {
            case ConfigKeyEnum.AutoBan:
                isRandom = AutoAcceptBanRandom;
                delayTime = AutoAcceptBanDelayTime;
                break;
            case ConfigKeyEnum.AutoSelect:
                isRandom = AutoAcceptSelectRandom;
                delayTime = AutoAcceptSelectDelayTime;
                //获取位置信息，根据位置信息选择英雄
                string position = Positions[UnRankedSelectPosition].Portrait;
                if (champ.MyTeam != null)
                {
                    TeamInfo? myInfo = champ.MyTeam.FirstOrDefault(p => p.CellId == champ.LocalPlayerCellId);
                    if (myInfo != null)
                        position = myInfo.AssignedPosition;
                }
                selectHeroes = selectHeroes.Where(p => p.Portrait == position);
                break;
            default:
                return;
        }
        selectHeroes = isRandom ? selectHeroes.OrderBy(_ => Random.Shared.Next()) : selectHeroes.OrderBy(p => p.Priority);
        lock (AutoHeroLock)
        {
            if (sessionTypeInt == _autoHeroState)
                return;
            Thread.Sleep(delayTime * 1000);
            if (sessionType == ConfigKeyEnum.AutoBan && !AutoAcceptBan || sessionType == ConfigKeyEnum.AutoSelect && !AutoAcceptSelect)
                return;
            foreach (SelectHero selectSessionHero in selectHeroes)
            {
                ApiResponse<string> result = _lcuService.ChampSelectSessionActionsAsync(action.Id, new SessionActionsRequest(selectSessionHero.ChampionId, action.Type)).Result;
                if (result.StatusCode != HttpStatusCode.OK)
                    continue;
                _autoHeroState = sessionTypeInt;
                break;
            }
        }
        await Task.CompletedTask;
    }

    #endregion

    #region 自动配置符文信息

    /// <summary>
    /// 选用英雄配置符文信息
    /// </summary>
    /// <param name="gameFlow"></param>
    private void AutoPerkSetHandleAsync(JToken gameFlow, string uri, string type)
    {
        SelectSummonersMessage? message = gameFlow.ToObject<SelectSummonersMessage>();
        if (!AutoPerk || message == null || Global.LocalPlayerCellId != message.CellId)
            return;
        SetPerkPage(message.ChampionId);
        Global.LocalPlayerCellId = null;
    }

    /// <summary>
    /// 设置符文页
    /// </summary>
    /// <param name="championId"></param>
    private void SetPerkPage(int championId)
    {
        HeroRune? heroRune = _dbService.Get<HeroRune>(p => p.ChampionId == championId);
        if (heroRune == null || string.IsNullOrWhiteSpace(heroRune.RuneJson))
            return;
        //获取符文信息
        ApiResponse<PerkCurrentPageResponse> getPerkResponse = _lcuService.GetPerkCurrentPageAsync().Result;
        long? perkPageId = getPerkResponse?.Content?.Id;
        //新符文页
        Dictionary<int, int[]>? selectRune = JsonConvert.DeserializeObject<Dictionary<int, int[]>>(heroRune.RuneJson);
        if (selectRune == null)
            return;
        PerkPageRequest request = new PerkPageRequest()
        {
            Name = heroRune.Name,
            SelectedPerkIds = selectRune.SelectMany(p => p.Value).Where(p => p > 0).ToList()
        };
        foreach (var item in selectRune.Where(item => item.Key != 2))
        {
            switch (item.Value.Length)
            {
                case 4:
                    request.PrimaryStyleId = item.Key;
                    break;
                case 3:
                    request.SubStyleId = item.Key;
                    break;
            }
        }
        _ = perkPageId == null ? _lcuService.CreatePerkPageAsync(request).Result : _lcuService.UpdatePerkPageAsync(perkPageId.Value, request).Result;
    }

    #endregion

    /// <summary>
    /// 重置客户端窗口
    /// </summary>
    public void ResetClientWindow()
    {
        _windowService.SetWindowsSize(1280, 720);
    }
}
