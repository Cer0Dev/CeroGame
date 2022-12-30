using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GameService.Models
{
    public class PlayerModel
    {
        public Guid Guid { get; set; } = Guid.Empty;
        private List<CardModel> _cards = new();
        public List<CardModel> Cards
        {
            get => _cards;
            set => _cards = value.OrderBy(x => x.Colour).ThenBy(x => x.Number).ToList();

        }
        public bool isPlayer => Cards.Any();
    }
}
