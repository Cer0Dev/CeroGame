using CeroGame.GameService.Enums;
using CeroGame.GameService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GameService.GameLogic
{
    public class GameMaster
    {

        public Guid Guid = Guid.NewGuid();

        public List<CardModel> MainDeck = new();
        public List<CardModel> MiddleDeck = new();
        public bool StandardDeck = true;
        // must be less than the amount of cards in deck
        public int StartingAmount = 20;
        public int MaxMiddleCards = 20;
        public int AmountOfPlayers = 2;
        public List<PlayerModel> Players = new();
        public List<CardModel> P1 = new();
        public List<CardModel> P2 = new();
        public List<int> Rotations = new() { 0 };
        public EventHandler RefreshNeeded;
        public string SessionIdKey { get; } = "UserId";
        public GameMaster()
        {
            GenerateDeck();
            GeneratePlayers();
            MiddleDeck = DealCards(1);

            //P1 = DealCards(StaringAmount);
            //P2 = DealCards(StaringAmount);

        }
        public void GenerateDeck()
        {
            if (StandardDeck)
            {
                foreach (var item in Enum.GetValues(typeof(Colours)).Cast<Colours>())
                {
                    Enumerable.Range(1, 9).ToList().ForEach(x => MainDeck.Add(new() { Colour = item, Number = x }));
                    Enumerable.Range(1, 9).ToList().ForEach(x => MainDeck.Add(new() { Colour = item, Number = x }));
                }
            }
        }

        private void GeneratePlayers()
        {
            for (int i = 0; i < AmountOfPlayers; i++)
            {
                Players.Add(new());
            }
        }

        public PlayerModel AddPlayer(Guid guid)
        {
            var EmptyPlayers = Players.Where(x => x.Guid == Guid.Empty);
            if (EmptyPlayers.Any() && !Players.Any( x=> x.Guid == guid))
            {
                Players[Players.IndexOf(EmptyPlayers.First())].Guid = guid;
                if(Players.Count() == AmountOfPlayers)
                {
                    Players.ForEach(x=> x.Cards = DealCards(StartingAmount).OrderBy(x => x.Colour).ThenBy(x => x.Number).ToList());
                }
                return Players.First();
            }

            return new() { Guid = guid };
            
        }

        public PlayerModel? GetPlayer(Guid guid) => Players.FirstOrDefault(x => x.Guid == guid);
        public List<CardModel> DealCards(int amount)
        {
            var cards = MainDeck.OrderBy(x => new Random().Next()).Take(amount).ToList();
            MainDeck = MainDeck.Except(cards).ToList();
            return cards;
        }

        public void PlayCard(CardModel card)
        {
            var b = P1;
            b.Remove(card);
            P1 = b;
            card.Active = false;
            MiddleDeck.Add(card);
            RefreshNeeded.Invoke(this, EventArgs.Empty);
        }

        public void DrawCard()
        {
            //for now just give to p1 until we add multi user handler
            var card = DealCards(1).FirstOrDefault();
            if (card is not null)
            {
                P1.Add(card);
            }
            RefreshNeeded.Invoke(this, EventArgs.Empty);

        }
    }
}
