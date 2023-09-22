using LOL.Assist.Core.DbModels;
using System.Collections.ObjectModel;

namespace LOL.Assist.Core.Models
{
    public class SelectHeroGroup
    {
        /// <summary>
        /// 位置信息
        /// </summary>
        public Position HeroPosition { get; set; }
        /// <summary>
        /// 列表信息
        /// </summary>
        public ObservableCollection<SelectHero> SelectHeroes { get; set; }

    }
}
