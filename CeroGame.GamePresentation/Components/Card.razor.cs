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
        [Parameter]
        public bool Played { get; set; }
        private string ColourString { get => Hidden ? "grey" : CardModel.Colour.ToString(); }
        private string CanBePlayedString { get => CanBePlayed ? "CanBePlayed" : "CardDisabled"; }
        private string _activeOffsetString { get => CardModel.Active && !Played && GM.CurrentPlayer == Deck.Player ? "-2" : "0"; }
        //Will need to modify this when multi user handler is implemented to check if the card exists in players hands played is temp variable
        public bool CanBePlayed { get => ((( ColourMatch || NumberMatch) && !Played && !Hidden && SpecialMatch) || CanPickup) && CurrentPlayer && !GM.GameOver; }
        private bool ColourMatch { get => (GM.MiddleDeck.Last().Colour == CardModel.Colour || CardModel.Colour == Colours.Any); }
        private bool NumberMatch { get => GM.MiddleDeck.Last().Number == CardModel.Number; }
        private bool CurrentPlayer { get => Deck?.Player == GM.CurrentPlayer; }
        private bool SpecialMatch { get => GM.MiddleDeck.Last().Number != 0 || GM.MiddleDeck.Last().Number == 0 && CardModel.CardType == GM.MiddleDeck.Last().CardType  || GM.MiddleDeck.Last().Number == 0 && ColourMatch && !GM.PlusNextPlayer; }
        public virtual void ToggleActive()
        {
            if (!CanBePlayed) return;
            if(CardModel.CardType != CardTypes.Standard && CardModel.Active && !CanPickup)
            {
                GM.SpecialActions[CardModel.CardType].Invoke();
            }
            if (CanPickup && CardModel.Active)
            {
                Deck.DrawCard();
                return;
            }
            else if (CardModel.Active)
            {
                Deck.PlayCard(CardModel);
                CardModel.Active = !CardModel.Active;
                return;
            }
            Deck?.DeactivateCards(CardModel);
            if (!CanPickup)
            {
                Deck?.DeactivateMainDeck();
            }
            CardModel.Active = !CardModel.Active;

            //GM G.RefreshNeeded.Invoke(this, EventArgs.Empty);


        }
    }
}
