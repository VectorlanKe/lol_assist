using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOL.Assist.Core.Models
{
    public class ChampSelectSessionRequest
    {
        public int SelectedSkinId { get; set; }
        public int Spell1Id { get; set; }
        public int Spell2Id { get; set; }
        public int WardSkinId { get; set; }
    }
}
