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
        private const double SpeedStep = 0.3;
        private const int DefaultBoardSize = 100;

        private Board _board;
        private int _boardSize;
        private DispatcherTimer _timer;
        private string _buttonLabel;
        private int _generationCount;
        private int _cellsBorn;
        private int _cellsDied;
        private bool _isRunning;
        private double _speed;
        private GraphicalRepresentation _representation;

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

        public ICommand ToggleSimulationCommand { get; }
        public ICommand StepCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RandomizeCommand { get; }
        public ICommand IncreaseSpeedCommand { get; }
        public ICommand DecreaseSpeedCommand { get; }
        public ICommand SetBoardSizeCommand { get;}
        public ICommand ToggleCellRepresentationCommand { get; }

        public GameViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;
            Speed = DefaultSpeed;
            BoardSize = DefaultBoardSize;
            Board = new Board(BoardSize);
            ButtonLabel = "Start";
            StepCommand = new RelayCommand(ExecuteStep);
            ToggleSimulationCommand = new RelayCommand(ToggleSimulation);
            ClearCommand = new RelayCommand(ClearBoard);
            RandomizeCommand = new RelayCommand(RandomizeBoard);
            SetBoardSizeCommand = new RelayCommand(SetBoardSize);
            ToggleCellRepresentationCommand = new RelayCommand(ToggleCellRepresentation);
            Representation = GraphicalRepresentation.Circle;
        }
        private void SetBoardSize()
        {
            if(BoardSize < 10 || BoardSize > 1000)
                return;

            Board = new Board(BoardSize);
            RandomizeBoard();
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
            {
                StopSimulation();
                ButtonLabel = "Start";
            }
            else
            {
                StartSimulation();
                ButtonLabel = "Stop";
            }
        }

        private void StartSimulation()
        {
            _isRunning = true;
            _timer.Start();
            CommandManager.InvalidateRequerySuggested();
        }

        private void StopSimulation()
        {
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
