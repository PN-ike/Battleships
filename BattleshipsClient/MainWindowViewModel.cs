using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;
using BattleshipsServer;

namespace BattleshipsClient
{
    public class MainWindowViewModel : BindableBase
    {
        Client c;// = new Client(); //TODO delete for actual playing
        private Game game = new Game();

        private Tile previousTurnedTile;

        private int _myHPLeft = 30;
        private int _enemyHPLeft = 30;
        
        private Tile[] _tiles = new Tile[100];
        private int _rows = 10;
        private int _cols = 10;
        private bool _isControlPanelEnabled = false;
        private bool _isStartButtonEnabled = true;
        private string _startButtonContent;

        private Input[] _inputs = new Input[10];
        private int setCounter = 0;
        private bool fleetSet = false;


        [STAThread]
        static void Main(string[] args)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.ShowDialog();
        }

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            UncoverTileCommand = new DelegateCommand<Tile>(UncoverTile);
            SetShipCommand = new DelegateCommand<Input>(SetShip);
            InitialiseGameTiles();
            InitialiseInput();
            StartButtonContent = "Start";
        }

        private void Start()
        {
            IsStartButtonEnabled = false;
            c = new Client();
            StartButtonContent = "Connected";

            new Thread(() =>
            {
                try
                {
                    String serverMessage;

                    int x = -1;
                    int y = -1;

                    while ((serverMessage = c.ReceiveMessageString()) != Message.CYA)
                    {
                        switch (serverMessage)
                        {

                            case Message.SET_YOUR_FLEET:
                                StartButtonContent = "Please Set Your Fleet";

                                foreach (Input i in Inputs)
                                {
                                    i.IsInputEnabled = true;
                                }

                                break;
                            case Message.SHOOT:
                                StartButtonContent = "Your Turn";
                                IsControlPanelEnabled = true;
                                break;
                            case Message.RECEIVE_COORDINATES:
                                x = -1;
                                y = -1;
                                c.ReceiveCoordinates(out x, out y);
                                break;
                            case Message.CHECK_DAMAGE:
                                bool newDamage = game.CheckDamage(x, y);

                                if (newDamage)
                                {
                                    MyHPLeft--; 
                                    bool isFinished = (MyHPLeft == 0);
                                    if (isFinished)
                                    {
                                        StartButtonContent = "YOU LOSE";
                                        c.SendMessage(Message.YOU_HAVE_WON);
                                    }
                                    else
                                    {
                                        c.SendMessage(Message.NEW_DAMAGE);
                                    }

                                }
                                else
                                {
                                    c.SendMessage(Message.MISSED);
                                }

                                break;
                            case Message.YOU_HAVE_WON:
                                EnemyHPLeft--;
                                previousTurnedTile.IsShip = true;
                                previousTurnedTile.Covered = false;
                                StartButtonContent = "YOU WIN";
                                break;
                            case Message.NEW_DAMAGE:
                                EnemyHPLeft--;
                                previousTurnedTile.IsShip = true;
                                previousTurnedTile.Covered = false;
                                break;
                            case Message.MISSED:
                                previousTurnedTile.IsShip = false;
                                previousTurnedTile.Covered = false;
                                break;
                            default:
                                break;
                        }
                    }

                    c.CloseConnection();

                    Console.Read();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
           ).Start();
        }


        private void InitialiseInput()
        {
            var inputs = new Input[10];
            for (int i = 0; i < 10; i++)
            {

                var input = new Input
                {
                    Row = i, 
                    Index = i, //TODO delete index
                    XValue = 0,
                    YValue = 0,
                    Position = Position.Horizontal
                };
                inputs[i] = input;
            }
            
            Inputs = inputs;
        }
        
        private void InitialiseGameTiles()
        {
            var tiles = new Tile[100];
            var index = 0;

            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    var tile = new Tile
                    {
                        Index = index,
                        Row = r,
                        Col = c
                    };
                    tiles[index] = tile;
                    index += 1;
                }
            }
            Tiles = tiles;
        }

        private void UncoverTile(Tile tile)
        {
            c.SendMessage(BitConverter.GetBytes(tile.Col));
            c.SendMessage(BitConverter.GetBytes(tile.Row)); 
            previousTurnedTile = tile; 
            IsControlPanelEnabled = false;
            StartButtonContent = "Enemies Turn";
            return;
        }

        private int GetLengthFromRowNumber(int rowNumber)
        {
            if (rowNumber == 0) { return 5; }
            else if (rowNumber == 1 || rowNumber == 2) { return 4; }
            else if (rowNumber == 3 || rowNumber == 4 || rowNumber == 5) { return 3; }
            else { return 2; }
        }

        private void SetShip(Input i)
        {
            if ((int)i.Position > 1) { return; } // check if Position enum is valid 

            bool setSuccessfully = false;
            Mark m = (Mark)(i.Row)+1;
            int length = GetLengthFromRowNumber(i.Row);
            Ship s = new Ship(i.XValue, i.YValue, i.Position, length, m);
            setSuccessfully = game.CheckValidShip(s);
            
            
            if (setSuccessfully)
            {
                i.IsInputEnabled = false;
                game.SetShip(s);
                setCounter++;
                if(setCounter == 10)
                {
                    fleetSet = true;
                }
            }

            if (fleetSet)
            {
                StartButtonContent = "Waiting for other player";
                c.SendMessage(Message.ACK);
            }
            return;
        }

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand<Tile> UncoverTileCommand { get; private set; }
        public DelegateCommand<Input> SetShipCommand{ get; private set; }

        public Tile[] Tiles
        {
            get { return _tiles; }
            set { SetProperty(ref _tiles, value); }
        }

        public Input[] Inputs
        {
            get { return _inputs; }
            set { SetProperty(ref _inputs, value); }
        }

        public string StartButtonContent
        {
            get { return _startButtonContent; }
            set { SetProperty(ref _startButtonContent, value); }
        }
        
        public bool IsControlPanelEnabled
        {
            get { return _isControlPanelEnabled; }
            set { SetProperty(ref _isControlPanelEnabled, value); }
        }

        public bool IsStartButtonEnabled
        {
            get { return _isStartButtonEnabled; }
            set { SetProperty(ref _isStartButtonEnabled, value); }
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                SetProperty(ref _rows, value);
            }
        }

        public int Columns
        {
            get { return _cols; }
            set
            {
                SetProperty(ref _cols, value);
            }
        }

        public int MyHPLeft
        {
            get { return _myHPLeft; }
            set { SetProperty(ref _myHPLeft, value); }
        }
        public int EnemyHPLeft
        {
            get { return _enemyHPLeft; }
            set { SetProperty(ref _enemyHPLeft, value); }
        }
    }

}
