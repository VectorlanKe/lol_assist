using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using LOL.Assist.App.ViewModels;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Models;
using Newtonsoft.Json;
using RuneGroup = LOL.Assist.App.Model.RuneGroup;

namespace LOL.Assist.App.Views
{
    /// <summary>
    /// RunePopupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RunePopupWindow
    {
        private readonly RunePopupViewModel _runePopupViewModel;

        public RunePopupWindow()
        {
            _runePopupViewModel = Ioc.Default.GetService<RunePopupViewModel>()!;
            DataContext = _runePopupViewModel;
            InitializeComponent();
        }
        /// <summary>
        /// 主符文页切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlPrimary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RuneGroup? addRuneGroup = e.AddedItems.Cast<RuneGroup>().FirstOrDefault();
            if (addRuneGroup != null)
            {
                addRuneGroup.PrimaryOpacity = 1;
                addRuneGroup.SecondVisibility = Visibility.Collapsed;
                addRuneGroup.Base.ForEach(p => p.IsChecked = false);
                addRuneGroup.Valiant.ForEach(p => p.IsChecked = false);
                addRuneGroup.Legend.ForEach(p => p.IsChecked = false);
                addRuneGroup.Combat.ForEach(p => p.IsChecked = false);
                RuneGroup? secondSelectItem = (RuneGroup)TabControlSecond.SelectedItem;
                if (secondSelectItem != null && secondSelectItem.Root.Id == addRuneGroup.Root.Id)
                    TabControlSecond.SelectedItem = _runePopupViewModel.ShowRunes.FirstOrDefault(p => Math.Abs(p.PrimaryOpacity - 1d) != 0);
            }
            RuneGroup? removeRuneGroup = e.RemovedItems.Cast<RuneGroup>().FirstOrDefault();
            if (removeRuneGroup == null)
                return;
            removeRuneGroup.PrimaryOpacity = 0.2;
            removeRuneGroup.SecondVisibility = Visibility.Visible;
            removeRuneGroup.Base.ForEach(p => p.IsChecked = false);
            removeRuneGroup.Valiant.ForEach(p => p.IsChecked = false);
            removeRuneGroup.Legend.ForEach(p => p.IsChecked = false);
            removeRuneGroup.Combat.ForEach(p => p.IsChecked = false);
        }
        /// <summary>
        /// 副符文页切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RuneGroup? addRuneGroup = e.AddedItems.Cast<RuneGroup>().FirstOrDefault();
            if (addRuneGroup != null)
            {
                addRuneGroup.SecondOpacity = 1;
                addRuneGroup.Base.ForEach(p => p.IsChecked = false);
                addRuneGroup.Valiant.ForEach(p => p.IsChecked = false);
                addRuneGroup.Legend.ForEach(p => p.IsChecked = false);
                addRuneGroup.Combat.ForEach(p => p.IsChecked = false);
            }
            RuneGroup? removeRuneGroup = e.RemovedItems.Cast<RuneGroup>().FirstOrDefault();
            if (removeRuneGroup == null)
                return;
            removeRuneGroup.SecondOpacity = 0.2;
        }

        /// <summary>
        /// 副符文选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            RuneGroup? secondSelectItem = (RuneGroup)TabControlSecond.SelectedItem;
            if (secondSelectItem == null)
                return;
            int selectId = int.Parse(radioButton.Tag?.ToString() ?? "0");
            List<RuneResponse?> runeResponses = new List<RuneResponse?>
            {
                secondSelectItem.Valiant.FirstOrDefault(p => p.IsChecked && p.Id != selectId),
                secondSelectItem.Legend.FirstOrDefault(p => p.IsChecked && p.Id != selectId),
                secondSelectItem.Combat.FirstOrDefault(p => p.IsChecked && p.Id != selectId)
            };
            IEnumerable<RuneResponse> selectRunes = runeResponses.Where(p => p != null)!;
            if (selectRunes.Count() >= 2)
                selectRunes.FirstOrDefault()!.IsChecked = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _runePopupViewModel.SaveSelectHeroRune();
            Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            _runePopupViewModel.SelectHeroRune = null;
            foreach (RuneGroup rootRuneGroup in _runePopupViewModel.ShowRunes)
            {
                rootRuneGroup.Root.IsChecked = false;
                rootRuneGroup.SecondOpacity = 0.2;
                rootRuneGroup.PrimaryOpacity = 0.2;
            }
            _runePopupViewModel.ShowGrowingRunes.Valiant.ForEach(p => p.IsChecked = false);
            _runePopupViewModel.ShowGrowingRunes.Legend.ForEach(p => p.IsChecked = false);
            _runePopupViewModel.ShowGrowingRunes.Combat.ForEach(p => p.IsChecked = false);
            base.OnClosed(e);
        }
        public void SetSelectHeroRune(HeroRune? heroRune)
        {
            if (heroRune == null)
                return;
            SaveButton.Visibility = Visibility.Hidden;
            _runePopupViewModel.SetSelectHeroRune(heroRune, TabControlPrimary, TabControlSecond);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectHeroWindow selectHeroWindow = new SelectHeroWindow
            {
                Owner = Window.GetWindow(this)
            };
            selectHeroWindow.InitializeComponent();
            selectHeroWindow.ShowDialog();
            Hero? hero = Global.Heroes.FirstOrDefault(p => p.HeroId == selectHeroWindow.SelectHeroId);
            if (hero == null)
                return;
            _runePopupViewModel.SelectHeroRune = new HeroRune
            {
                ChampionId = hero.HeroId,
                HeadPortrait = hero.HeadPortrait,
                Name = hero.Name,
                KeyWords = hero.KeyWords
            };
        }
    }
}
