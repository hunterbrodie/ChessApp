using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System;

namespace ChessApp.Classes
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Player>().Wait();
            _database.CreateTableAsync<Game>().Wait();
        }

        public Task<int> DeletePlayer(Player _player)
        {
            return _database.DeleteAsync<Player>(_player.ID);
        }

        public Task<int> ResetPlayerTable()
        {
            return _database.DeleteAllAsync<Player>();
        }

        public Task<int> ResetGameTable()
        {
            return _database.DeleteAllAsync<Game>();
        }

        public Task<List<Player>> GetPlayerListAsync()
        {
            return _database.Table<Player>().ToListAsync();
        }

        public Task<int> SavePlayerAsync(Player _player)
        {
            return _database.InsertAsync(_player);
        }

        public Task<Player> GetPlayerAsync(int pk)
        {
            return _database.GetAsync<Player>(pk);
        }

        public Task<List<Game>> GetGameListAsync()
        {
            return _database.Table<Game>().ToListAsync();
        }

        public Task<int> SaveGameAsync(Game _game)
        {
            return _database.InsertAsync(_game);
        }

        public Task<int> UpdatePlayerAsync(Player _player)
        {
            return _database.UpdateAsync(_player);
        }

        public Task<int> RecalculateRatings()
        {
            List<Game> gameList = _database.Table<Game>().ToListAsync().Result;
            List<Player> playerList = _database.Table<Player>().ToListAsync().Result;
            double[] ratings = new double[playerList.Count];
            for (int x = 0; x < ratings.Length; x++)
            {
                ratings[x] = 100;
            }
            for (int x = 0; x < gameList.Count; x++)
            {
                int wI = GetPlayerPosition(playerList, gameList[x].p1ID);
                int lI = GetPlayerPosition(playerList, gameList[x].p2ID);

                gameList[x].p1Rating = ratings[wI];
                gameList[x].p2Rating = ratings[lI];

                double eA = 1 / (1 + Math.Pow(10, (playerList[lI].Rating - playerList[wI].Rating) / 40));
                double eB = 1 - eA;
                eA = 8 * (gameList[x].p1Result - eA);
                eB = 8 * ((1 - gameList[x].p1Result) - eB);

                ratings[wI] = Math.Round(gameList[x].p1Rating + eA, 2);
                ratings[lI] = Math.Round(gameList[x].p2Rating + eB, 2);

            }
            for (int x = 0; x < ratings.Length; x++)
            {
                playerList[x].Rating = Math.Round(ratings[x], 2);
            }
            return _database.UpdateAllAsync(playerList);
        }

        private int GetPlayerPosition(List<Player> players, int ID)
        {
            for (int x = 0; x < players.Count; x++)
            {
                if (players[x].ID == ID)
                {
                    return x;
                }
            }
            return -1;
        }

    }
}
