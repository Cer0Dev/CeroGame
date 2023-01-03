using CeroGame.GamePresentation.Core;
using CeroGame.GameService.GameLogic;
using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GamePresentation.Components
{
    public partial class Deck : DeckBase
    {

        [Parameter]
        public int CardsPerRow { get; set; } = 7;
        [Parameter]
        public bool Hidden { get; set; } = true;
        protected double _topOffsetMultiplyer = 1.3;
        protected double _leftOffsetMultiplyer = 3;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Player is not null)
            {
                Cards = GM.GetCards(Player);

            }


        }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (GM.GameOver)
            {
                await jsruntime.InvokeVoidAsync("alert", GM.CurrentPlayer.Guid + " Has won the game");
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Player is not null)
            {
                Cards = GM.GetCards(Player);

            }
        }


        protected List<CardModel> _cards = new();


        protected List<CardModel> Cards
        {
            get => _cards;
            set => _cards = value.OrderBy(x => x.Colour).ThenBy(x => x.Number).ToList();

        }
    }
}
