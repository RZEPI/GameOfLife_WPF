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
        private void Redraw_Click(object sender, RoutedEventArgs e)
        { 
            DrawCells();
        }
        private Shape CellRepresentation(GameViewModel.GraphicalRepresentation representation, Cell cell)
        {
            if (representation == GameViewModel.GraphicalRepresentation.Circle)
                return new Ellipse
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = cell.IsAlive ? Brushes.Black : Brushes.White,
                    Tag = cell
                };
            else
                return new Rectangle
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = cell.IsAlive ? Brushes.Black : Brushes.White,
                    Tag = cell
                };
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
                    var representation = gameViewModel.Representation;
                    var shape = CellRepresentation(representation, cell);

                    cell.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(Cell.IsAlive))
                        {
                            shape.Fill = cell.IsAlive ? Brushes.Black : Brushes.White;
                        }
                    };

                    Canvas.SetLeft(shape, column * cellSize);
                    Canvas.SetTop(shape, row * cellSize);
                    GameBoard.Children.Add(shape);
                }
            }
        }
    }
}