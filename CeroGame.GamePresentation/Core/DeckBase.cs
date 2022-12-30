using CeroGame.GamePresentation.Components;
using CeroGame.GameService.GameLogic;
using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GamePresentation.Core
{
    public class DeckBase : ComponentBase
    {
        [Parameter]
        public GameMaster GM { get; set; }
        [Parameter]
        public Table? Table { get; set; }
        protected List<CardModel> _cards { get; set; } = new();

        public void DeactivateCards(CardModel cardToIgnore)
        {
            _cards.ForEach(x => { if (x.GuidId != cardToIgnore.GuidId) { x.Active = false; } });
            StateHasChanged();
        }

        public void PlayCard(CardModel card)
        {
            GM?.PlayCard(card);

        }
        public void DrawCard()
        {
            GM?.DrawCard();
        }
    }
}
