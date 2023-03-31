using CeroGame.GamePresentation.Components;
using CeroGame.GameService.GameLogic;
using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CeroGame.GamePresentation.Core
{
    public class DeckBase : ComponentBase
    {
        [Parameter]
        public GameMaster GM { get; set; } = default!;
        [Parameter]
        public Table? Table { get; set; }
        [Parameter]
        public PlayerModel Player { get; set; } = default!;
        [Inject]
        public IJSRuntime jsruntime { get; set; } = default!;

        public void DeactivateCards(CardModel? cardToIgnore = null)
        {
            GM.DeactivateCards(Player, cardToIgnore);
            StateHasChanged();
        }
        public void DeactivateMainDeck()
        {
            GM.DeactivateCards(Player);
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
