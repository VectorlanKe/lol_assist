using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using RuneGroup = LOL.Assist.App.Model.RuneGroup;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.Core.IServices;

namespace LOL.Assist.App.ViewModels
{
    public partial class RunePopupViewModel : ObservableObject
    {
        private static readonly IEnumerable<string> ValiantStyleNames = new List<string> { "英武", "预谋", "宝物", "巧具", "蛮力" };
        private static readonly IEnumerable<string> LegendStyleNames = new List<string> { "传说", "追踪", "卓越", "未来", "抵抗" };
        private static readonly IEnumerable<string> CombatStyleNames = new List<string> { "战斗", "狩猎", "威能", "超越", "生机" };
        /// <summary>
        /// 符文
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<RuneGroup> _showRunes;
        /// <summary>
        /// 成长符文
        /// </summary>
        [ObservableProperty]
        private RuneGroup _showGrowingRunes;

        [ObservableProperty]
        private HeroRune _selectHeroRune;

        private readonly IDbService _dbService;
        /// <summary>
        /// 
        /// </summary>
        public RunePopupViewModel()
        {
            _dbService = Ioc.Default.GetService<IDbService>()!;
            RefreshReloadRune();
        }

        public void RefreshReloadRune()
        {
            IEnumerable<RuneGroup> runeGroup = Global.Runes
                .Where(p => !string.IsNullOrWhiteSpace(p.Key) && string.IsNullOrWhiteSpace(p.SlotLabel) &&
                            string.IsNullOrWhiteSpace(p.StyleName)).Select(runeResponse =>
                {
                    List<IGrouping<string, RuneResponse>> groupBy = Global.Runes
                        .Where(p => p.StyleName == runeResponse.Name)
                        .GroupBy(p => p.SlotLabel)
                        .ToList();

                    return new RuneGroup
                    {
                        Root = runeResponse,
                        Base = groupBy.FirstOrDefault(p => p.Key == "基石")!.ToList(),
                        Valiant = groupBy.FirstOrDefault(p => ValiantStyleNames.Contains(p.Key))!.ToList(),
                        Legend = groupBy.FirstOrDefault(p => LegendStyleNames.Contains(p.Key))!.ToList(),
                        Combat = groupBy.FirstOrDefault(p => CombatStyleNames.Contains(p.Key))!.ToList(),
                    };
                });
            ShowRunes = new ObservableCollection<RuneGroup>(runeGroup);
            ShowGrowingRunes = new RuneGroup
            {
                Valiant = GetGrowingRues(new int[] { 5008, 5005, 5007 }),
                Legend = GetGrowingRues(new int[] { 5008, 5002, 5003 }),
                Combat = GetGrowingRues(new int[] { 5001, 5002, 5003 }),
            };
            SelectHeroRune = null;
        }

        private List<RuneResponse> GetGrowingRues(int[] ids)
        {
            return ids.Select(p =>
            {
                RuneResponse source = Global.Runes.FirstOrDefault(px => px.Id == p)!;
                return new RuneResponse
                {
                    GroupName = source.GroupName,
                    Icon = source.Icon,
                    Id = source.Id,
                    IsChecked = source.IsChecked,
                    Key = source.Key,
                    LongDesc = source.LongDesc,
                    Name = source.Name,
                    Opacity = source.Opacity,
                    ShortDesc = source.ShortDesc,
                    SlotLabel = source.SlotLabel,
                    StyleName = source.StyleName,
                    Tooltip = source.Tooltip,
                };
            }).ToList();
        }

        /// <summary>
        /// 获取英雄选中服务信息
        /// </summary>
        /// <returns></returns>
        public void SaveSelectHeroRune()
        {
            if (SelectHeroRune == null|| SelectHeroRune.ChampionId <=0)
                return;
            RuneGroup defaultRuneGroup = new RuneGroup() { Root = new RuneResponse() };
            RuneGroup primaryOpacity = ShowRunes.FirstOrDefault(p => Math.Abs(p.PrimaryOpacity - 1) == 0) ?? defaultRuneGroup;
            RuneGroup secondRunes = ShowRunes.FirstOrDefault(p => Math.Abs(p.SecondOpacity - 1) == 0) ?? defaultRuneGroup;
            Dictionary<int, int[]> selectRune = new Dictionary<int, int[]>()
            {
                {
                    primaryOpacity.Root.Id, new int[4]
                    {
                        GetRuneCheckId(primaryOpacity.Base),
                        GetRuneCheckId(primaryOpacity.Valiant),
                        GetRuneCheckId(primaryOpacity.Legend),
                        GetRuneCheckId(primaryOpacity.Combat),
                    }
                },
                {
                    secondRunes.Root.Id==0?1:secondRunes.Root.Id, new int[3]
                    {
                        GetRuneCheckId(secondRunes.Valiant),
                        GetRuneCheckId(secondRunes.Legend),
                        GetRuneCheckId(secondRunes.Combat),
                    }
                },
                {
                    2, new int[3]
                    {
                        GetRuneCheckId(ShowGrowingRunes.Valiant),
                        GetRuneCheckId(ShowGrowingRunes.Legend),
                        GetRuneCheckId(ShowGrowingRunes.Combat),
                    }
                }
            };
            string runeJson = JsonConvert.SerializeObject(selectRune);
            SelectHeroRune!.RuneJson = runeJson;
            _dbService.InsertOrUpdate(SelectHeroRune);
        }
        /// <summary>
        /// 设置英雄选中服务信息
        /// </summary>
        /// <returns></returns>
        public void SetSelectHeroRune(HeroRune? heroRune, TabControl primaryTab, TabControl secondTab)
        {
            SelectHeroRune = heroRune;
            if (heroRune == null || string.IsNullOrWhiteSpace(heroRune.RuneJson))
                return;
            Dictionary<int, int[]>? selectRune = JsonConvert.DeserializeObject<Dictionary<int, int[]>>(heroRune.RuneJson);
            if (selectRune is not { Count: 3 })
                return;
            foreach (var kvp in selectRune)
            {
                //成长符文
                if (kvp is { Key: 2, Value.Length: 3 })
                {
                    SetRuneCheckId(ShowGrowingRunes.Valiant, kvp.Value[0]);
                    SetRuneCheckId(ShowGrowingRunes.Legend, kvp.Value[1]);
                    SetRuneCheckId(ShowGrowingRunes.Combat, kvp.Value[2]);
                    continue;
                }

                RuneGroup? rootRune = ShowRunes.FirstOrDefault(p => p.Root.Id == kvp.Key);
                if (rootRune == null)
                    continue;
                switch (kvp.Value.Length)
                {
                    //主系
                    case 4:
                        primaryTab.SelectedItem = rootRune;
                        SetRuneCheckId(rootRune.Base, kvp.Value[0]);
                        SetRuneCheckId(rootRune.Valiant, kvp.Value[1]);
                        SetRuneCheckId(rootRune.Legend, kvp.Value[2]);
                        SetRuneCheckId(rootRune.Combat, kvp.Value[3]);
                        break;
                    //副系
                    case 3:
                        secondTab.SelectedItem = rootRune;
                        SetRuneCheckId(rootRune.Valiant, kvp.Value[0]);
                        SetRuneCheckId(rootRune.Legend, kvp.Value[1]);
                        SetRuneCheckId(rootRune.Combat, kvp.Value[2]);
                        break;
                }
            }
        }

        private int GetRuneCheckId(IEnumerable<RuneResponse>? runes)
        {
            if (runes == null)
                return 0;
            return runes.FirstOrDefault(p => p.IsChecked)?.Id ?? 0;
        }
        private void SetRuneCheckId(IEnumerable<RuneResponse>? runes, int id)
        {
            if (runes == null || id == 0)
                return;
            RuneResponse? setRune = runes.FirstOrDefault(p => p.Id == id);
            if (setRune == null)
                return;
            setRune.IsChecked = true;
        }
    }
}
