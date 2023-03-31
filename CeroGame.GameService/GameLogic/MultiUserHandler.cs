using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GameService.GameLogic
{
    public class MultiUserHandler
    {

        private List<GameMaster> Games = new();
        public double delayForDestruction = 120;
        public GameMaster AddGame(GameMaster game)
        {
            Games.Add(game);
            return game;
        }

        public GameMaster? FetchGame(Guid guid)
        {
            return Games.FirstOrDefault(x => x.Guid == guid);
        }

        public async Task<bool> DestroyGameAsync(GameMaster gm)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayForDestruction));
            return Games.Remove(gm);
        }
       
    }
}
