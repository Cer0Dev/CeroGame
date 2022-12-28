using CeroGame.GameService.Enums;
using CeroGame.GameService.GameLogic;
using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GamePresentation.Components
{
    public partial class Card
    {
        [Parameter]
        public CardModel CardModel { get; set; } 
        [Parameter]
        public Deck? Deck { get; set; }
        [Parameter]
        public double LeftOffset { get; set; }
        [Parameter]
        public double topOffset { get; set; }
        [Parameter]
        public bool Hidden { get; set; } = true;
        [Parameter]
        public GameMaster GM { get; set; }
        private string ColourString { get => Hidden ? "grey" : CardModel.Colour.ToString(); }
        private string CanBePlayedString { get => CanBePlayed ? "CanBePlayed" : "CardDisabled"; }
        //private string DisabledCardString { get => CanBePlayed ? string.Empty : "Dark"; }
        private string _activeOffsetString { get => CardModel.Active ? "-2" : "0"; }
        public bool CanBePlayed { get => (GM.MiddleDeck.Last().Colour == CardModel.Colour || GM.MiddleDeck.Last().Number == CardModel.Number) && Deck is not null && !Hidden; }
        public void ToggleActive()
        {
            if (Hidden || !CanBePlayed) return;
            if (CardModel.Active)
            {
                Deck.PlayCard(CardModel);
                CardModel.Active = !CardModel.Active;

                return;
            }
            Deck?.DeactivateCards(CardModel);
            CardModel.Active = !CardModel.Active;
            StateHasChanged();
        }

    }
}
