using System;
using System.Text;

namespace BattleshipsClient
{

    class Game
    {
        private Mark[,] gameField = new Mark[10, 10]; //y, x
        private Ship[] fleet = new Ship[1]; //TODO change back

        public Game()
        {
            initGameField();
        }

        public Boolean isFinished()
        {
            Boolean isFinished = true;

            foreach (Ship s in fleet) {
                if (s.hp != 0)
                {
                    isFinished = false;
                }
            }

            return isFinished;
        }

        public Boolean checkDamage(int x, int y)
        {
            Boolean newDamage = false;

            if (gameField[y, x] == Mark.X)
            {
                Console.WriteLine("Ship was already hit here");
            }
            else if (gameField[y, x] == Mark._)
            {
                Console.WriteLine("Missed");
            }
            else
            {
                Ship s = getShip(x, y);
                s.hp--;
                gameField[y, x] = Mark.X;
                printGameField();
                newDamage = true;
            }
            return newDamage;
        }


        private Ship getShip(int x, int y)
        {
            Mark m = gameField[y, x];
            return fleet[(int)m - 1];
        }

        public int intYValue(char y)
        {
            if (Char.IsLower(y)) {
                return y - 97;
            }
            else
            {
                return y - 65;
            }
        }
        
        public void shoot(out int x, out int y)
        {
            enterCoordinates(out x, out y, validSettingInput);
      
        }

        public void setTestFleet()
        {
            int x = 0;
            int y = 0;
            int length = 5;
            
            Ship s = new Ship(x, y, Position.horizontal, length, Mark.A);
            /*
            fleet[0] = setShip(s);
            x = 5;
            y = 5;
            length = 4;
            s = new Ship(x, y, Position.vertical, length, Mark.B);
            fleet[1] = setShip(s);
            x = 0;
            y = 3;
            s = new Ship(x, y, Position.vertical, length, Mark.C);
            fleet[2] = setShip(s);
            x = 5;
            y = 3;
            length = 3;
            s = new Ship(x, y, Position.horizontal, length, Mark.D);
            fleet[3] = setShip(s);
            x = 7;
            y = 0;
            s = new Ship(x, y, Position.horizontal, length, Mark.E);
            fleet[4] = setShip(s);
            x = 9;
            y = 7;
            s = new Ship(x, y, Position.vertical, length, Mark.F);
            fleet[5] = setShip(s);
            x = 0;
            y = 9;
            */
            length = 2;
            /*
            s = new Ship(x, y, Position.horizontal, length, Mark.G);
            fleet[6] = setShip(s);
            x = 3;
            y = 3;
            s = new Ship(x, y, Position.vertical, length, Mark.H);
            fleet[7] = setShip(s);
            x = 2;
            y = 6;
            s = new Ship(x, y, Position.vertical, length, Mark.I);
            fleet[8] = setShip(s);
            */
            x = 7;
            y = 5;
            s = new Ship(x, y, Position.horizontal, length, Mark.A);
            fleet[0] = setShip(s);
        }

        public void setFleet()
        {
            int length = 5;
            int nTypes = 4;
            int nShips = 1;
            Mark m = Mark.A;
            Boolean isValidShip = false;
            printGameField();

            for (int i = 0; i < nTypes; i++)
            {
                for (int j = 0; j < nShips; j++)
                {
                    Ship s = null;
                    while (!isValidShip)
                    {
                        Console.WriteLine("Setting ship with length " + length);
                        Position p = enterPostioning();
                        int x = -1;
                        int y = -1;
                        enterCoordinates(out x, out y, validSettingInput);
                        s = new Ship(x, y, p, length, m);
                        isValidShip = checkValidShip(s);
                    }
                    fleet[i+j] = setShip(s); //TODO check if fleet[i+j] is correct
                    m++;
                    printGameField();
                    isValidShip = false;
                }
                nShips++;
                length--;
            }

            return;
        }

        private bool checkValidShip(Ship s)
        {
      
            //TODO check if ship is out of gameField
            //TODO check if there is a neighbouring ship

            return true;
        }

        public Ship setShip(Ship s)
        {
            if (s.positioning == Position.horizontal)
            {
                for (int i = 0; i < s.length; i++)
                {
                    gameField[s.y, s.x + i] = s.type;
                }
            }
            else
            {
                for (int i = 0; i < s.length; i++)
                {
                    gameField[s.y + i, s.x] = s.type;
                }
            }
            return s;
        }

        public Position enterPostioning()
        {
            Console.WriteLine("Please enter positioning (h = horizontal, v = vertical)");
            char c = Console.ReadKey().KeyChar;

            while (c != 'h' && c != 'v')
            {
                Console.WriteLine("\nIncorrect Input");
                Console.WriteLine("Please enter positioning (h = horizontal, v = vertical)");
                c = Console.ReadKey().KeyChar;
            }

            if (c == 'h')
            {
                return Position.horizontal;
            }
            else
            {
                return Position.vertical;
            }
        }

        public delegate Boolean checkInput(int x);

        public void enterCoordinates(out int x, out int y, checkInput checkInput)
        {

            Console.WriteLine("Please enter valid X-Coordinate");
            x = (int)Char.GetNumericValue(Console.ReadKey().KeyChar);

            while (!checkInput(x))
            {
                Console.WriteLine("\nIncorrect Input");
                Console.WriteLine("Please enter valid X-Coordinate");
                x = (int)Char.GetNumericValue(Console.ReadKey().KeyChar);
            }

            char c;
            Console.WriteLine("\nPlease enter valid Y-Coordinate");
            c = Console.ReadKey().KeyChar;
            y = intYValue(c);

            while (!checkInput(y))
            {
                Console.WriteLine("\nIncorrect Input");
                Console.WriteLine("Please enter valid Y-Coordinate");
                c = Console.ReadKey().KeyChar;
                y = intYValue(c);
            }
        }

        private Boolean validSettingInput(int i)
        {
            return (i >= 0 && i <= 10);
        }

        public String printGameField() //TODO visibility
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n");

            sb.Append("  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | \n");
            sb.Append("  |---|---|---|---|---|---|---|---|---|---|\n");

            char c = 'A';

            for (int i = 0; i < gameField.GetLength(0); i++)

            {
                sb.Append(c + " | ");
                c++;
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    sb.Append(gameField[i, j] + " | ");
                }
                sb.Append("\n");
                sb.Append("  |---|---|---|---|---|---|---|---|---|---|\n");
            }

            String field = sb.ToString();

            Console.WriteLine(field);

            return field;
        }

        private void initGameField()
        {
            for (int i = 0; i < gameField.GetLength(0); i++)
            {
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    gameField[i, j] = Mark._;
                }
            }
        }
    }

    enum Position { horizontal = 1, vertical = 2 };

    enum Mark { X = 0, A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8, I = 9, J = 10, _ = -1 }

    class Ship
    {
        public Mark type;
        public int hp;
        public Position positioning;

        public Position Positioning
        {
            get => positioning;
            private set => positioning = value;
        }

        public int length;

        public int Length
        {
            get => length;
            private set => length = value;
        }


        public int x;

        public int X
        {
            get => x;
            private set => x = value;

        }

        public int y;

        public int Y
        {
            get => y;
            private set => y = value;
        }

        public Ship(int x, int y, Position positioning, int length, Mark m)
        {
            this.x = x;
            this.y = y;
            this.length = length;
            this.positioning = positioning;
            this.type = m;
            this.hp = this.length;

        }
    }
}

