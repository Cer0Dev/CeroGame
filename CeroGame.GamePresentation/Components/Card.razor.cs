using CeroGame.GamePresentation.Core;
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
        public DeckBase? Deck { get; set; }
        [Parameter]
        public double LeftOffset { get; set; }
        [Parameter]
        public double topOffset { get; set; }
        [Parameter]
        public bool Hidden { get; set; } = true;
        [Parameter]
        public GameMaster GM { get; set; }
        [Parameter]
        public bool CanPickup { get; set; }
        //played is temp
        [Parameter]
        public bool Played { get; set; }
        private string ColourString { get => Hidden ? "grey" : CardModel.Colour.ToString(); }
        private string CanBePlayedString { get => CanBePlayed ? "CanBePlayed" : "CardDisabled"; }
        private string _activeOffsetString { get => CardModel.Active && !Played ? "-2" : "0"; }
        //Will need to modify this when multi user handler is implemented to check if the card exists in players hands played is temp variable
        public bool CanBePlayed { get => ((GM.MiddleDeck.Last().Colour == CardModel.Colour || GM.MiddleDeck.Last().Number == CardModel.Number) && !Played && !Hidden) || CanPickup; }
        public void ToggleActive()
        {
            if (!CanBePlayed) return;

            if (CanPickup && CardModel.Active)
            {
                //change when adding multi user handler
                Deck.DrawCard();

            }
            else if (CardModel.Active)
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
