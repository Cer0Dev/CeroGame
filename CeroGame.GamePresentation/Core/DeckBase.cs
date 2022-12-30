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
        [Parameter]
        public PlayerModel Player { get; set; }

        public void DeactivateCards(CardModel cardToIgnore)
        {
            GM.DeactivateCards(Player, cardToIgnore);
            StateHasChanged();
        }
        public void PlayCard(CardModel card)
        {
            GM?.PlayCard(Player,card);

        }
        public void DrawCard()
        {
            GM?.DrawCard(Player);
        }
    }
}
