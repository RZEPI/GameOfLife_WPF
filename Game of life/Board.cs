using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Game_of_life
{
    public class Board
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Cell[,] Cells { get; private set; }

        public Board(int rows) : this(rows, rows){}

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            
            CreateCellArray();

            Randomize();
        }

        private void CreateCellArray()
        {
            Cells = new Cell[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cells[i, j] = new Cell();
                }
            }
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

        public void SaveToFile()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("board.txt"))
            {
                file.WriteLine(Rows);
                file.WriteLine(Columns);
                for (int i = 0; i < Rows; i++)
                {
                    string line = "";
                    for (int j = 0; j < Columns; j++)
                    {
                        line += Cells[i, j].IsAlive ? "1" : "0";
                    }
                    file.WriteLine(line);
                }
            }
        }

        public int LoadFromFile()
        {
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("board.txt"))
                {
                    Rows = int.Parse(file.ReadLine());
                    Columns = int.Parse(file.ReadLine());
                    CreateCellArray();
                    for (int i = 0; i < Rows; i++)
                    {
                        string line = file.ReadLine();
                        for (int j = 0; j < Columns; j++)
                        {
                            Cells[i, j].IsAlive = line[j] == '1';
                        }
                    }
                }
            }
            catch(Exception)
            {
                Clear();
            }
            return Rows;
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
