using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game_of_life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int cellSize = 10;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new GameViewModel();
            DrawCells();
        }

        private void DrawCells()
        {
            GameBoard.Children.Clear();

            var gameViewModel = (GameViewModel)DataContext;
            var board = gameViewModel.Board;
            for(int row = 0; row < board.Rows; row++)
            {
                for(int column = 0; column < board.Columns; column++)
                {
                    var cell = board.Cells[row, column];
                    var rectangle = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = cell.IsAlive ? Brushes.Black : Brushes.White,
                        Tag = cell
                    };
                    cell.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(Cell.IsAlive))
                        {
                            rectangle.Fill = cell.IsAlive ? Brushes.Green : Brushes.White;
                        }
                    };

                    Canvas.SetLeft(rectangle, column * cellSize);
                    Canvas.SetTop(rectangle, row * cellSize);
                    GameBoard.Children.Add(rectangle);
                }
            }
        }
    }
}