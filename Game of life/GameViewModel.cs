using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace Game_of_life
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private const int BaseSpeed = 1000;
        private const double DefaultSpeed = 1;
        private const int DefaultBoardSize = 100;
        public const int DefaultCellSize = 10;
        public const int ZoomCellSize = 100;

        private Board _board;
        private int _boardSize;
        private DispatcherTimer _timer;
        private string _buttonLabel;
        private int _generationCount;
        private int _cellsBorn;
        private int _cellsDied;
        private bool _isRunning;
        private Tuple<int, int> _zoom;
        private double _speed;
        private GraphicalRepresentation _representation;
        private int _cellSize;

        public Board Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged(nameof(Board));
            }
        }
        public int BoardSize
        {
            get => _boardSize;
            set
            {
                if (_boardSize != value)
                {
                    _boardSize = value;
                    OnPropertyChanged(nameof(BoardSize));
                }
            }
        }

        public int GenerationCount
        {
            get => _generationCount;
            set
            {
                _generationCount = value;
                OnPropertyChanged(nameof(GenerationCount));
            }
        }

        public int CellSize
        {
            get => _cellSize;
            set
            {
                _cellSize = value;
                OnPropertyChanged(nameof(CellSize));
            }
        }

        public GraphicalRepresentation Representation
        {
            get => _representation;
            set
            {
                _representation = value;
                OnPropertyChanged(nameof(Representation));
            }
        }

        public int CellsBorn
        {
            get => _cellsBorn;
            set
            {
                _cellsBorn = value;
                OnPropertyChanged(nameof(CellsBorn));
            }
        }

        public int CellsDied
        {
            get => _cellsDied;
            set
            {
                _cellsDied = value;
                OnPropertyChanged(nameof(CellsDied));
            }
        }

        public double Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
                ChangeSpeed();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }
        public string ButtonLabel
        {
            get => _buttonLabel;
            set
            {
                _buttonLabel = value;
                OnPropertyChanged(nameof(ButtonLabel));
            }
        }
        
        public Tuple<int, int>Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                OnPropertyChanged(nameof(Zoom));
            }
        }

        public ICommand ToggleSimulationCommand { get; }
        public ICommand StepCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RandomizeCommand { get; }
        public ICommand IncreaseSpeedCommand { get; }
        public ICommand DecreaseSpeedCommand { get; }
        public ICommand SetBoardSizeCommand { get;}
        public ICommand ToggleCellRepresentationCommand { get; }
        public ICommand SaveToFileCommand { get; }
        public ICommand LoadFromFileCommand { get; }

        public GameViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;
            Speed = DefaultSpeed;
            BoardSize = DefaultBoardSize;
            Board = new Board(BoardSize);
            Representation = GraphicalRepresentation.Circle;
            CellSize = DefaultCellSize;

            ButtonLabel = "Start";
            StepCommand = new RelayCommand(ExecuteStep);
            ToggleSimulationCommand = new RelayCommand(ToggleSimulation);
            ClearCommand = new RelayCommand(ClearBoard);
            RandomizeCommand = new RelayCommand(RandomizeBoard);
            SetBoardSizeCommand = new RelayCommand(SetBoardSize);
            ToggleCellRepresentationCommand = new RelayCommand(ToggleCellRepresentation);
            SaveToFileCommand = new RelayCommand(SaveToFile);
            LoadFromFileCommand = new RelayCommand(LoadFromFile);

        }
        private void SetBoardSize()
        {
            if(BoardSize < 10 || BoardSize > 1000)
                return;

            Board = new Board(BoardSize);
        }
        public void ToggleZoom(double x=0, double y=0)
        {
            if (Zoom == null)
            {
                CellSize = ZoomCellSize;
                int column = (int)x / DefaultCellSize;
                int row = (int)y / DefaultCellSize;
                Zoom = new Tuple<int, int>(column, row);
            }
            else
            {
                CellSize = DefaultCellSize;
                Zoom = null;
            }
        }

        private void ExecuteStep()
        {
            var stepStats = Board.Step();
            GenerationCount++;
            CellsBorn = stepStats.Item1;
            CellsDied = stepStats.Item2;
        }
        private void ToggleSimulation()
        {
            if (_isRunning)
                StopSimulation();
            else
                StartSimulation();
        }

        private void StartSimulation()
        {
            ButtonLabel = "Stop";
            _isRunning = true;
            _timer.Start();
            CommandManager.InvalidateRequerySuggested();
        }

        private void StopSimulation()
        {
            ButtonLabel = "Start";
            _isRunning = false;
            _timer.Stop();
            CommandManager.InvalidateRequerySuggested();
        }
        private void ChangeSpeed()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(BaseSpeed / Speed);
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            ExecuteStep();
        }

        private void ClearBoard()
        {
            StopSimulation();
            Board.Clear();
            GenerationCount = 0;
            CellsBorn = 0;
            CellsDied = 0;
        }

        private void RandomizeBoard()
        {
            ClearBoard();
            Board.Randomize();
        }
        private void ToggleCellRepresentation()
        {
            Representation = Representation == GraphicalRepresentation.Circle ? GraphicalRepresentation.Rectangle : GraphicalRepresentation.Circle;
            
        }

        private void SaveToFile()
        {
            Board.SaveToFile();
        }
        private void LoadFromFile()
        {
            if(_isRunning)
                StopSimulation();

            BoardSize = Board.LoadFromFile();
            OnPropertyChanged(nameof(Board));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public enum GraphicalRepresentation
        {
            Rectangle,
            Circle
        }
   }
}
