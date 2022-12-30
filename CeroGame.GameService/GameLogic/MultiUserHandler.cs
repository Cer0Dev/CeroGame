using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GameService.GameLogic
{
    public static class MultiUserHandler
    {

        private static List<GameMaster> Games = new();

        public static GameMaster AddGame(GameMaster game)
        {
            Games.Add(game);
            return game;
        }

        public static GameMaster? FetchGame(Guid guid)
        {
            return Games.FirstOrDefault(x => x.Guid == guid);
        }
       
    }
}
