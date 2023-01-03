using CeroGame.GameService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GameService.Models
{
    public class CardModel
    {
        public Guid GuidId { get; set; } = Guid.NewGuid();
        public int Number { get; set; }
        public string? Icon { get; set; }
        public string? Image { get; set; }
        public string? Text { get; set; }
        public bool isNumber { get => string.IsNullOrEmpty(Icon) && string.IsNullOrEmpty(Image) && string.IsNullOrEmpty(Text); }
        public Colours Colour { get; set; }
        public CardTypes CardType { get; set; } = CardTypes.Standard;
        public bool Active { get; set; }
    }
}
