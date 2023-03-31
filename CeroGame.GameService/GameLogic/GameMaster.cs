using CeroGame.GameService.Enums;
using CeroGame.GameService.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public int StartingAmount = 3;
        //public int StartingAmount = 20;
        public int MaxMiddleCards = 15;
        public int AmountOfPlayers = 2;
        public List<PlayerModel> Players = new();
        public int OpponentCardCount = 5;
        public bool PlusNextPlayer { get; set; }

        public PlayerModel? CurrentPlayer;
        public PlayerModel? NextPlayer
        {
            get
            {
                var index = Players.IndexOf(CurrentPlayer);
                return index + 1 >= AmountOfPlayers ? Players[0] : Players[index + 1];
            }
        }
        public List<int> Rotations = new() { 0 };
        public EventHandler? RefreshNeeded;
        public EventHandler<bool>? GameEnded;
        public bool GameOver;
        public Dictionary<CardTypes, Action> SpecialActions { get; set; } = new();
        private int PlusCount;
        private MultiUserHandler MultiUserHandler { get; set; }
        public GameMaster(MultiUserHandler multiUserHandler)
        {
            GenerateDeck();
            GeneratePlayers();
            MiddleDeck = DealCards(1,true);
            MultiUserHandler = multiUserHandler;

        }
        public void GenerateDeck()
        {
            if (StandardDeck)
            {
                SpecialActions.Add(CardTypes.PlusTwo, PlusAction);
                SpecialActions.Add(CardTypes.Skip, SkipAction);
                foreach (var item in Enum.GetValues(typeof(Colours)).Cast<Colours>().Where(x => x != Colours.Any).ToList())
                {
                    Enumerable.Range(1, 9).ToList().ForEach(x => MainDeck.Add(new() { Colour = item, Number = x }));
                    Enumerable.Range(1, 9).ToList().ForEach(x => MainDeck.Add(new() { Colour = item, Number = x }));
                    MainDeck.Add(new() { Colour = item, Text = "+2", CardType = CardTypes.PlusTwo });
                    MainDeck.Add(new() { Colour = item, Text = "+2", CardType = CardTypes.PlusTwo });
                    MainDeck.Add(new() { Colour = item, Icon = "fa-solid fa-ban", CardType = CardTypes.Skip });
                    MainDeck.Add(new() { Colour = item, Icon = "fa-solid fa-ban", CardType = CardTypes.Skip });

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
                    CurrentPlayer = Players[new Random().Next(0, Players.Count())];
                    //foreach (var item in Players)
                    //{
                    //    Players[Players.IndexOf(item)].Cards.Add(new() { Colour = Colours.Red, CardType = CardTypes.PlusTwo, Text = "+2" });
                    //    Players[Players.IndexOf(item)].Cards.Add(new() { Colour = Colours.Red, CardType = CardTypes.Standard, Number = 1 });
                    //}
                    RefreshNeeded?.Invoke(this, EventArgs.Empty);

                }

                return Players[Players.IndexOf(EmptyPlayers.First())];
            }
            Players.Add(new() { Guid = guid });
            return Players.Last();
        }

        public PlayerModel? GetPlayer(Guid guid) => Players.FirstOrDefault(x => x.Guid == guid);
        public List<CardModel> GetCards(PlayerModel player) => Players.First(x => x == player).Cards;
        public List<CardModel> DealCards(int amount, bool middledeck = false)
        {

            if (amount > MainDeck.Count())
            {
                MiddleDeck.AddRange(MiddleDeck.Take(MiddleDeck.Count() - 1));
                MiddleDeck.RemoveRange(0, MiddleDeck.Count() - 1);
            }
            List<CardModel> cards = new();
            if (!middledeck)
            {
                cards = MainDeck.OrderBy(x => new Random().Next()).Take(amount).ToList();

            }
            else
            {
                cards = MainDeck.Where(x => x.CardType == CardTypes.Standard).OrderBy(x => new Random().Next()).Take(amount).ToList();

            }
            MainDeck = MainDeck.Except(cards).ToList();
            return cards;
        }

        public void PlayCard(PlayerModel player, CardModel card)
        {
            var playerLookUp = Players[Players.IndexOf(player)];
            var success = playerLookUp.Cards.Remove(card);
            MiddleDeck.Add(card);
            MainDeck.ForEach(x => x.Active = false);
            if (!playerLookUp.HasCards)
            {
                GameOver = true;
                GameEnded?.Invoke(this,GameOver);
                _ = MultiUserHandler.DestroyGameAsync(this);
            }
            else
            {
                UpdateCurrentPlayer();
            }
            RefreshNeeded?.Invoke(this, EventArgs.Empty);
        }


        public void UpdateCurrentPlayer()
        {
            if (!MainDeck.Any())
            {
                MiddleDeck.AddRange(MiddleDeck.Take(MiddleDeck.Count() - 1));
                MiddleDeck.RemoveRange(0, MiddleDeck.Count() - 1);
            }
            if (PlusNextPlayer && NextPlayer is not null && !NextPlayer.Cards.Any(x => x.CardType.ToString().Contains("plus", StringComparison.OrdinalIgnoreCase)))

            {
                DrawCard(NextPlayer);
            }
            CurrentPlayer = NextPlayer;

        }

        public void DrawCard(PlayerModel player)
        {
            var card = DealCards(!PlusNextPlayer ? 1 : PlusCount);
            if (PlusNextPlayer)
            {
                PlusNextPlayer = false;
                PlusCount = 0;
            }
            if (card is not null)
            {
                Players[Players.IndexOf(player)].Cards.AddRange(card);
            }
            MainDeck.ForEach(x => x.Active = false);

            UpdateCurrentPlayer();
            RefreshNeeded?.Invoke(this, EventArgs.Empty);

        }

        public void DeactivateCards(PlayerModel player, CardModel? cardToIgnore = null)
        {
            Players[Players.IndexOf(player)].Cards
            .ForEach(x => { if (x != cardToIgnore) { x.Active = false; } });

            if (cardToIgnore is null)
            {
                MainDeck.ForEach(x => x.Active = false);
            }
            RefreshNeeded?.Invoke(this, EventArgs.Empty);

        }

        public void DeactivateMainDeck()
        {
            MainDeck.ForEach(x => x.Active = false);
            RefreshNeeded?.Invoke(this, EventArgs.Empty);

        }

        public void PlusAction()
        {
            var card = CurrentPlayer?.Cards.FirstOrDefault(x => x.CardType.ToString().Contains("plus", StringComparison.OrdinalIgnoreCase));
            if (card is not null)
            {
                if (card.CardType == CardTypes.PlusTwo)
                {
                    PlusCount += 2;
                }
            }
            PlusNextPlayer = true;
            RefreshNeeded?.Invoke(this, EventArgs.Empty);

        }

        public void SkipAction()
        {
            UpdateCurrentPlayer();
            RefreshNeeded?.Invoke(this, EventArgs.Empty);

        }
    }
}