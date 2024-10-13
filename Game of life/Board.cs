using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_life
{
    public class Board
    {
        public int Rows { get; }
        public int Columns { get; }
        public Cell[,] Cells { get; }

        public Board(int rows) : this(rows, rows){}

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new Cell[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Cells[i, j] = new Cell();
                }
            }
            Randomize();
        }

        public void Randomize()
        {
            Random random = new Random();
            foreach (var cell in Cells)
            {
                cell.IsAlive = random.Next(0, 2) == 1;
            }
        }

        public void Clear()
        {
            foreach (var cell in Cells)
            {
                cell.IsAlive = false;
            }
        }

        public Tuple<int,int> Step()
        {
            bool[,] nextState = new bool[Rows, Columns];
            int cellsBorn = 0;
            int cellsDied = 0;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    int aliveNeighbors = CountAliveNeighbors(i, j);
                    if (Cells[i, j].IsAlive)
                    {
                        bool staysAlive = aliveNeighbors == 2 || aliveNeighbors == 3;
                        if (!staysAlive)
                            cellsDied++;
                        nextState[i, j] = staysAlive;
                    }
                    else
                    {
                        bool becomesAlive = aliveNeighbors == 3;
                        if (becomesAlive)
                            cellsBorn++;
                        nextState[i, j] = becomesAlive;
                    }
                }
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cells[i, j].IsAlive = nextState[i, j];
                }
            }
            return Tuple.Create(cellsBorn, cellsDied);
        }

        private int CountAliveNeighbors(int row, int column)
        {
            int count = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    if (i >= 0 && i < Rows && j >= 0 && j < Columns && !(i == row && j == column) && Cells[i, j].IsAlive)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
