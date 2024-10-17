using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game_of_life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GameViewModel();
            var viewModel = (GameViewModel)DataContext;
            viewModel.PropertyChanged += (s, e) =>
            {
                
                if (e.PropertyName == nameof(GameViewModel.Board) 
                || e.PropertyName == nameof(GameViewModel.Representation)
                || e.PropertyName == nameof(GameViewModel.BoardSize)
                || e.PropertyName == nameof(GameViewModel.Zoom))
                {
                    DrawCells();
                }
            };
            DrawCells();
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(GameBoard);

            double x = mousePosition.X;
            double y = mousePosition.Y;
            
            var viewModel = (GameViewModel)DataContext;
            viewModel.ToggleZoom(x, y);
        }
        private Shape CellRepresentation(GameViewModel.GraphicalRepresentation representation, Cell cell, int cellSize)
        {
            var brush = cell.IsAlive ? Brushes.Black : Brushes.White;
            if (representation == GameViewModel.GraphicalRepresentation.Circle)
                return new Ellipse
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = brush,
                    Tag = cell
                };
            else
                return new Rectangle
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = brush,
                    Tag = cell
                };
        }

        private void DrawCells(int colStart, int colEnd, int rowStart, int rowEnd)
        {
            var gameViewModel = (GameViewModel)DataContext;
            var board = gameViewModel.Board;
            var cellSize = gameViewModel.CellSize;
            var representation = gameViewModel.Representation;

            for (int row = rowStart; row < rowEnd; row++)
            {
                for (int column = colStart; column < colEnd; column++)
                {
                    var cell = board.Cells[row, column];
                    var shape = CellRepresentation(representation, cell, cellSize);

                    cell.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(Cell.IsAlive))
                        {
                            shape.Fill = cell.IsAlive ? Brushes.Black : Brushes.White;
                        }
                    };

                    Canvas.SetLeft(shape, (column - colStart) * cellSize);
                    Canvas.SetTop(shape, (row - rowStart) * cellSize);
                    GameBoard.Children.Add(shape);
                }
            }
        }

        private void DrawCells()
        {
            GameBoard.Children.Clear(); 

            var gameViewModel = (GameViewModel)DataContext;   
            var board = gameViewModel.Board;
            var zoom = gameViewModel.Zoom;

            if (zoom != null)
            {
                int rowStart = zoom.Item2 - 2 ;
                int rowEnd = zoom.Item2 + 3;
                int columnStart = zoom.Item1 - 2;
                int columnEnd = zoom.Item1 + 3;
                if(rowStart < 0)
                    rowStart = 0;
                if(rowEnd > board.Rows)
                    rowEnd = board.Rows;
                if(columnStart < 0)
                    columnStart = 0;
                if(columnEnd > board.Columns)
                    columnEnd = board.Columns;

                DrawCells(columnStart, columnEnd, rowStart, rowEnd);
            }
            else
            {
                DrawCells(0, board.Columns, 0, board.Rows);
            }
        }
    }
}