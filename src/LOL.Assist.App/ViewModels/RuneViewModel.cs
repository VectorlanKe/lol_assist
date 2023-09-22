using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.App.Model;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.IServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using LOL.Assist.Core.Models;

namespace LOL.Assist.App.ViewModels
{
    public partial class RuneViewModel : ObservableObject
    {
        [ObservableProperty] private string _searchText = string.Empty;
        [ObservableProperty] private ObservableCollection<HeroRuneGroup>? _heroRunes;
        private readonly IDbService _dbService;

        public RuneViewModel()
        {
            _dbService = Ioc.Default.GetService<IDbService>()!;
            RefreshHeroRune();
        }
        partial void OnSearchTextChanged(string value)
        {
            RefreshHeroRune();
        }

        public void RemoveHeroRune(int championId)
        {
            _dbService.Delete<HeroRune>(p => p.ChampionId == championId);
            HeroRuneGroup? removeHero = HeroRunes?.FirstOrDefault(p => p.HeroRuneBase.ChampionId == championId);
            if (removeHero == null)
                return;
            HeroRunes!.Remove(removeHero);
        }
        public void RefreshHeroRune()
        {
            IList<HeroRune> data = _dbService.GetAll<HeroRune>(string.IsNullOrWhiteSpace(SearchText) ? p => true : p => p.KeyWords.Contains(SearchText) || p.Name.Contains(SearchText));
            HeroRunes = new ObservableCollection<HeroRuneGroup>(data.Select(p =>
            {
                HeroRuneGroup heroRuneGroup = new HeroRuneGroup
                {
                    HeroRuneBase = p,
                    Primary = new List<RuneResponse>(),
                    SecondGrowing = new List<RuneResponse>(),
                };
                if (string.IsNullOrWhiteSpace(p.RuneJson))
                    return heroRuneGroup;
                Dictionary<int, int[]>? selectRune = JsonConvert.DeserializeObject<Dictionary<int, int[]>>(p.RuneJson);
                if (selectRune is not { Count: 3 })
                    return heroRuneGroup;

                foreach (var kvp in selectRune)
                {
                    //成长符文
                    if (kvp is { Key: 2, Value.Length: 3 })
                    {
                        heroRuneGroup.SecondGrowing.AddRange(kvp.Value
                            .Where(runeId => runeId != 0)
                            .Select(px => Global.Runes.FirstOrDefault(pj => px == pj.Id) ?? new RuneResponse()));
                        continue;
                    }

                    RuneResponse? rootRune = Global.Runes.FirstOrDefault(px => px.Id == kvp.Key);
                    if (rootRune == null)
                        continue;
                    switch (kvp.Value.Length)
                    {
                        //主系
                        case 4:
                            heroRuneGroup.Primary.Add(rootRune);
                            heroRuneGroup.Primary.AddRange(kvp.Value
                                .Where(runeId => runeId != 0)
                                .Select(px => Global.Runes.FirstOrDefault(pj => px == pj.Id) ?? new RuneResponse()));
                            break;
                        //副系
                        case 3:
                            heroRuneGroup.SecondGrowing.Add(rootRune);
                            heroRuneGroup.SecondGrowing.AddRange(kvp.Value
                                .Where(runeId => runeId != 0)
                                .Select(px => Global.Runes.FirstOrDefault(pj => px == pj.Id) ?? new RuneResponse()));
                            break;
                    }
                }
                return heroRuneGroup;
            }));
        }
    }
}
