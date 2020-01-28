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
            //get { return getRecentRating().Result; }
            get;
            set;
        }

        public override string ToString()
        {
            return PName + " (" + Rating + ")";
        }

    }
}
