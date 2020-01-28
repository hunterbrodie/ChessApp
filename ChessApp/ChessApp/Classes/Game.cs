using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ChessApp.Classes
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        public string gDate
        {
            get;
            set;
        }
        public int p1ID
        {
            get;
            set;
        }
        public int p2ID
        {
            get;
            set;
        }
        public double p1Rating
        {
            get;
            set;
        }
        public double p2Rating
        {
            get;
            set;
        }

        public double p1Result
        {
            get;
            set;
        }

        public string Disp
        {
            //get { return ToString(); }
            get { return p1Rating + " beat " + p2Rating; }
        }

        public override string ToString()
        {
            return getDisp().Result;
        }

        public async Task<string> getDisp()
        {
            Player tempPlayer = await App.Database.GetPlayerAsync(p1ID);
            string disp = tempPlayer.PName + " (" + p1Rating + ") " + " beat ";
            tempPlayer = await App.Database.GetPlayerAsync(p2ID);
            disp += tempPlayer.PName + " (" + p2Rating + ")";
            return disp;
        }

    }
}
