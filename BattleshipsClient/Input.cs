﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient
{
    public class Input : BindableBase
    {
        private int _index;
        private int _row;
        private Boolean _isInputEnabled = false;

        public Boolean IsInputEnabled
        {
            get { return _isInputEnabled; }
            set { SetProperty(ref _isInputEnabled, value); }
        }
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        public int Row
        {
            get { return _row; }
            set { SetProperty(ref _row, value); }
        }
        
        private Position _position;
        private int _xValue;
        private int _yValue;

        public int XValue
        {
            get { return _xValue; }
            set { SetProperty(ref _xValue, value); }
        }
        public int YValue
        {
            get { return _yValue; }
            set { SetProperty(ref _yValue, value); }
        }
        public Position Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }
    }
}
