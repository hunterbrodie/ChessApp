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
        public DateTime gDate
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

        public string shortGDate
        {
            get { return gDate.ToShortDateString(); }
        }

        public string Disp
        {
            get
            {
                string disp = "";

                disp += App.Database.GetPlayerAsync(p1ID).Result.PName + " (" + p1Rating + ")";

                if (p1Result == 1)
                {
                    disp += " won against ";
                }
                else if (p1Result == 0)
                {
                    disp += " lost to ";
                }
                else
                {
                    disp += " tied with ";
                }

                disp += App.Database.GetPlayerAsync(p2ID).Result.PName + " (" + p2Rating + ")";

                disp += " on " + gDate.ToString();

                return disp;
            }
        }

        public string shortDisp
        {
            get
            {
                string disp = "";

                disp += App.Database.GetPlayerAsync(p1ID).Result.PName;

                if (p1Result == 1)
                {
                    disp += " won against ";
                }
                else if (p1Result == 0)
                {
                    disp += " lost to ";
                }
                else
                {
                    disp += " tied with ";
                }

                disp += App.Database.GetPlayerAsync(p2ID).Result.PName;

                return disp;
            }
        }

        public override string ToString()
        {
            return Disp;
        }

    }
}
