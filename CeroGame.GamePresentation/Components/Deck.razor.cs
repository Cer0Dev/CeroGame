using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GamePresentation.Components
{
    public partial class Deck
    {

        [Parameter]
        public int CardsPerRow { get; set; } = 7;
        [Parameter]
        public bool Hidden { get; set; } = true;
        private List<CardModel> _cards { get; set; } = new();
        
        [Parameter]
        public List<CardModel> Cards
        {
            get => _cards;
            set => _cards = value.OrderBy(x=> x.Colour).ThenBy(x=> x.Number).ToList();
        }
    }
}
