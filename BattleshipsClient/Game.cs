using System;
using System.Text;

namespace BattleshipsClient
{
    class Game
    {
        private const int FIELD_SIZE = 10;

        private Mark[,] gameField = new Mark[FIELD_SIZE, FIELD_SIZE]; //y, x
        
        public Game()
        {
            initGameField();
        }

        public Boolean isFinished()
        {
            return false;
        }

        public Boolean checkDamage(int x, int y)
        {
            Boolean newDamage = false;

            if (gameField[y, x] == Mark._)
            {
                Console.WriteLine("Missed");
            }
            else
            {
                newDamage = true;
            }
            return newDamage;
        }

        public int intYValue(char y)
        {
            if (Char.IsLower(y))
            {
                return y - 97;
            }
            else
            {
                return y - 65;
            }
        }
        //TODO change BOLEAN to bool everywhere
        public bool checkValidShip(Ship s)
        {
            printGameField();
            
            if (s.Position == Position.horizontal)
            {

                if ((s.X + (s.Length-1)) >= FIELD_SIZE)
                {
                    return false;
                }
                for (int i = 0; i < s.Length; i++)
                {
                    if (gameField[s.Y, s.X+ i] != Mark._)
                    {
                        return false;
                    }
                }

            }
            else
            {
                if ((s.Y + (s.Length - 1)) >= FIELD_SIZE)
                {
                    return false;
                }
                for (int i = 0; i < s.Length; i++)
                {
                    if (gameField[s.Y + i, s.X] != Mark._)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        public Ship setShip(Ship s)
        {
            if (s.Position == Position.horizontal)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    gameField[s.Y, s.X + i] = s.Type;
                }
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    gameField[s.Y + i, s.X] = s.Type;
                }
            }
            return s;
        }

        public String printGameField()
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

    public enum Position { horizontal = 0, vertical = 1 }; //TODO changed if problems occur check here

    enum Mark { A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8, I = 9, J = 10, _ = 0 } //TODO changed if problems occur check here

    class Ship
    {
        public Mark Type { get; private set; }
        public Position Position { get; private set; }

        public int Length { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Ship(int x, int y, Position positioning, int length, Mark m)
        {
            this.X = x;
            this.Y = y;
            this.Length = length;
            this.Position = positioning;
            this.Type = m;
        }
    }
}

