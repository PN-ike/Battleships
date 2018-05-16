using System;
using Prism.Mvvm;

namespace BattleshipsClient
{
    public class Tile : BindableBase
    {
       private bool _covered = true;
        private int _index;
        private int _row;
        private int _col;
        private string _status = "";
        private bool _isShip = false;


        public bool IsShip
        {
            get { return _isShip; }
            set { SetProperty(ref _isShip, value); }
        }

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        public string DisplayTileContent
        {
            get { if (IsShip)
                {
                    return "HIT";
                }
                else
                {
                    return "MISSED";
                }
            }
        }


        public bool Covered
        {
            get { return _covered; }
            set { SetProperty(ref _covered, value); }
        }

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public int Row
        {
            get { return _row; }
            set { SetProperty(ref _row, value); }
        }

        public int Col
        {
            get { return _col; }
            set { SetProperty(ref _col, value); }
        }
        public static int CellSize { get { return 23; } }

    }

}
