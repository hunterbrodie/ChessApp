using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System;
using System.Linq;

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

        public void DeletePlayer(Player _player)
        {
            List<Game> allGames = _database.Table<Game>().ToListAsync().Result;
            for (int x = 0; x < allGames.Count; x++)
            {
                if (allGames[x].p1ID == _player.ID || allGames[x].p2ID == _player.ID)
                {
                    _database.DeleteAsync<Game>(allGames[x].ID).Wait();
                }
            }
            _database.DeleteAsync<Player>(_player.ID).Wait();
            RecalculateRatings().Wait();
        }

        public void DeleteGame(Game _game)
        {
            _database.DeleteAsync<Game>(_game.ID).Wait();
            RecalculateRatings().Wait();
        }

        public void DeleteGameFromID(int ID)
        {
            _database.DeleteAsync<Game>(ID).Wait();
            RecalculateRatings().Wait();
        }

        public void ResetPlayerTable()
        {
            _database.DeleteAllAsync<Player>().Wait();
        }

        public void ResetGameTable()
        {
            _database.DeleteAllAsync<Game>().Wait();
            RecalculateRatings().Wait();
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
            List<Game> gameList = _database.Table<Game>().ToListAsync().Result.OrderBy(g => g.gDate).ToList();
            List<Player> playerList = _database.Table<Player>().ToListAsync().Result;

            for (int x  = 0; x < playerList.Count; x++)
            {
                playerList[x].Rating = 1000;
            }

            for (int x = 0; x < gameList.Count; x++)
            {
                int p1Pos = GetPlayerPosition(playerList, gameList[x].p1ID);
                int p2Pos = GetPlayerPosition(playerList, gameList[x].p2ID);

                gameList[x].p1Rating = playerList[p1Pos].Rating;
                gameList[x].p2Rating = playerList[p2Pos].Rating;

                double eA = 1 / (1 + Math.Pow(10, (playerList[p2Pos].Rating - playerList[p1Pos].Rating) / 400));
                double eB = 1 - eA;

                eA = Math.Round(32 * (gameList[x].p1Result - eA), 2);
                eB = Math.Round(32 * ((1 - gameList[x].p1Result) - eB), 2);

                playerList[p1Pos].Rating += eA;
                playerList[p2Pos].Rating += eB;

            }

            _database.UpdateAllAsync(gameList).Wait();
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
