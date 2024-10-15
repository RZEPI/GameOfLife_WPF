using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;

namespace Game_of_life
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private Board _board;
        private int _boardSize;
        private string _buttonLabel;
        private int _generationCount;
        private int _cellsBorn;
        private int _cellsDied;
        private bool _isRunning;
        private int _speed;
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

        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
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
            BoardSize = 100;
            Board = new Board(BoardSize);
            ButtonLabel = "Start";
            StepCommand = new RelayCommand(ExecuteStep, CanExecuteStep);
            ToggleSimulationCommand = new RelayCommand(ToggleSimulation);
            ClearCommand = new RelayCommand(ClearBoard);
            RandomizeCommand = new RelayCommand(RandomizeBoard);
            IncreaseSpeedCommand = new RelayCommand(() => Speed++);
            DecreaseSpeedCommand = new RelayCommand(() => Speed--);
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
            (StepCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
        private bool CanExecuteStep()
        {
            return _isRunning;
        }

        private void StartSimulation()
        {
            _isRunning = true;
        }

        private void StopSimulation()
        {
            _isRunning = false;
        }

        private void ClearBoard()
        {
            Board.Clear();
            GenerationCount = 0;
            CellsBorn = 0;
            CellsDied = 0;
        }

        private void RandomizeBoard()
        {
            _isRunning = false;
            this.ClearBoard();
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
