using CeroGame.GameService.GameLogic;
using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        [Parameter]
        public GameMaster? GM { get; set; }
        [Parameter]
        public Table? Table { get; set; }
        private List<CardModel> _cards { get; set; } = new();
        private double _topOffsetMultiplyer = 1.3;
        private double _leftOffsetMultiplyer = 3;

        [Parameter]
        public List<CardModel> Cards
        {
            get => _cards;
            set => _cards = value.OrderBy(x => x.Colour).ThenBy(x => x.Number).ToList();
            
        }

        public void DeactivateCards(CardModel cardToIgnore)
        {
            _cards.ForEach(x => { if (x.GuidId != cardToIgnore.GuidId) { x.Active = false; } });
            StateHasChanged();
        }

        public void PlayCard(CardModel card)
        {
            GM?.PlayCard(card);
            Table.Refresh();
        }
    }
}
