using CeroGame.GameService.Enums;
using CeroGame.GameService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        public int StaringAmount = 20;

        public List<CardModel> P1 = new();
        public List<CardModel> P2 = new();
        public GameMaster()
        {
            GenerateDeck();
            P1 = DealCards(StaringAmount);
            P2 = DealCards(StaringAmount);
            MiddleDeck = DealCards(1);

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
            MiddleDeck.Add(card);
        }
    }
}
