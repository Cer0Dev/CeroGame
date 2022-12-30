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
        public PlayerModel CurrentPlayer;
        public List<int> Rotations = new() { 0 };
        public EventHandler RefreshNeeded;
        public string SessionIdKey { get; } = "UserId";
        public GameMaster()
        {
            GenerateDeck();
            GeneratePlayers();
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

        private void GeneratePlayers()
        {
            for (int i = 0; i < AmountOfPlayers; i++)
            {
                Players.Add(new());
            }
        }

        public PlayerModel AddPlayer(Guid guid)
        {
            var EmptyPlayers = Players.Where(x => x.Guid == Guid.Empty).ToList();
            var Player = Players.FirstOrDefault(x => x.Guid == guid);
            if (Player is not null)
            {
                return Player;
            }
            if (EmptyPlayers.Any())
            {
                Players[Players.IndexOf(EmptyPlayers.First())].Guid = guid;
                if (!Players.Any(x => x.Guid == Guid.Empty))
                {
                    Players.ForEach(x => x.Cards = DealCards(StartingAmount).OrderBy(x => x.Colour).ThenBy(x => x.Number).ToList());
                    CurrentPlayer = Players[new Random().Next(0, Players.Count()) ];
                    RefreshNeeded.Invoke(this, EventArgs.Empty);
                }
                return Players[Players.IndexOf(EmptyPlayers.First())];
            }
            Players.Add(new() { Guid = guid });
            return Players.Last();
        }

        public PlayerModel? GetPlayer(Guid guid) => Players.FirstOrDefault(x => x.Guid == guid);
        public List<CardModel> GetCards(PlayerModel player) => Players.First(x => x == player).Cards;
        public List<CardModel> DealCards(int amount)
        {
            var cards = MainDeck.OrderBy(x => new Random().Next()).Take(amount).ToList();
            MainDeck = MainDeck.Except(cards).ToList();
            return cards;
        }

        public void PlayCard(PlayerModel player, CardModel card)
        {

            var success = Players.FirstOrDefault(x => x.Cards.Any(y => y == card))?.Cards.Remove(card);
            MiddleDeck.Add(card);
            MainDeck.ForEach(x => x.Active = false);
            UpdateCurrentPlayer();
            RefreshNeeded.Invoke(this, EventArgs.Empty);
        }
        public void UpdateCurrentPlayer()
        {
            var index = Players.IndexOf(CurrentPlayer);
            CurrentPlayer = index + 1 >= AmountOfPlayers ? Players[0] : Players[index + 1];

        }

        public void DrawCard(PlayerModel player)
        {
            //for now just give to p1 until we add multi user handler
            var card = DealCards(1).FirstOrDefault();
            if (card is not null)
            {
                Players[Players.IndexOf(player)].Cards.Add(card);
            }
            MainDeck.ForEach(x => x.Active = false);
            UpdateCurrentPlayer();
            RefreshNeeded.Invoke(this, EventArgs.Empty);

        }

        public void DeactivateCards(PlayerModel player, CardModel? cardToIgnore = null)
        {
            Players[Players.IndexOf(player)].Cards
            .ForEach(x => { if (x != cardToIgnore) { x.Active = false; } });

        }
    }
}
