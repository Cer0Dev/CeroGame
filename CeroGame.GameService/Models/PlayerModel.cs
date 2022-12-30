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
        public List<CardModel> Cards { get; set; } = new();
    }
}
