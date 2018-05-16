using System;
using System.Text;

namespace BattleshipsClient
{
    internal class Game
    {
        private const int FIELD_SIZE = 10;
        private Mark[,] gameField = new Mark[FIELD_SIZE, FIELD_SIZE]; //y, x
        
        internal Game()
        {
            InitGameField();
        }
        
        internal bool CheckDamage(int x, int y)
        {
            return !(gameField[y, x] == Mark._);
        }
        
        internal bool CheckValidShip(Ship s)
        {
            PrintGameField();
            
            if (s.Position == Position.Horizontal)
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

        internal Ship SetShip(Ship s)
        {
            if (s.Position == Position.Horizontal)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    gameField[s.Y, s.X + i] = s.Mark;
                }
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    gameField[s.Y + i, s.X] = s.Mark;
                }
            }
            return s;
        }

        private String PrintGameField()
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

        private void InitGameField()
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

    public enum Position { Horizontal = 0, Vertical = 1 }; 

    enum Mark { A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8, I = 9, J = 10, _ = 0 } 

    class Ship
    {
        public Mark Mark { get; private set; }
        public Position Position { get; private set; }

        public int Length { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Ship(int x, int y, Position position, int length, Mark mark)
        {
            X = x;
            Y = y;
            Length = length;
            Position = position;
            Mark = mark;
        }
    }
}

