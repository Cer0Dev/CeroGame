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
        public Colours Colour { get; set; }
        public bool Active { get; set; }
    }
}
