using System;
using System.Collections.Generic;
using SQLite;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Classes
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        public string PName
        {
            get;
            set;
        }
        public double Rating
        {
            get;
            set;
        }

        public string Wlt
        {
            get
            {
                int[] wlt = { 0, 0, 0 };
                List<Game> _gameList = App.Database.GetGameListAsync().Result;
                for (int x = 0; x < _gameList.Count; x++)
                {
                    if (_gameList[x].p1ID == ID)
                    {
                        if (_gameList[x].p1Result == 1)
                        {
                            wlt[0]++;
                        }
                        else if (_gameList[x].p1Result == 0)
                        {
                            wlt[1]++;
                        }
                        else
                        {
                            wlt[2]++;
                        }

                    }
                    else if (_gameList[x].p2ID == ID)
                    {
                        if (_gameList[x].p1Result == 1)
                        {
                            wlt[1]++;
                        }
                        else if (_gameList[x].p1Result == 0)
                        {
                            wlt[0]++;
                        }
                        else
                        {
                            wlt[2]++;
                        }
                    }
                }

                return wlt[0] + "/" + wlt[1] + "/" + wlt[2];
            }
        }

        public override string ToString()
        {
            return PName + " (" + Rating + ")";
        }

    }
}
