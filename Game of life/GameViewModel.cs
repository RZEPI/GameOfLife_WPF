using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;

namespace Game_of_life
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private Board _board;
        private string _buttonLabel;
        private int _generationCount;
        private int _cellsBorn;
        private int _cellsDied;
        private bool _isRunning;
        private int _speed;
        private GraphicalRepresentation _representation = GraphicalRepresentation.Circle;

        public Board Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged(nameof(Board));
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

        public GameViewModel()
        {
            Board = new Board(100);
            ButtonLabel = "Start";
            StepCommand = new RelayCommand(ExecuteStep, CanExecuteStep);
            ToggleSimulationCommand = new RelayCommand(ToggleSimulation);
            ClearCommand = new RelayCommand(ClearBoard);
            RandomizeCommand = new RelayCommand(RandomizeBoard);
            IncreaseSpeedCommand = new RelayCommand(() => Speed++);
            DecreaseSpeedCommand = new RelayCommand(() => Speed--);
            SetBoardSizeCommand = new RelayCommand<int>(SetBoardSize);
        }
        private void SetBoardSize(int size)
        {
            Board = new Board(size);
            ClearBoard();
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
            this.ClearBoard();
            Board.Randomize();
            _isRunning = false;
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
