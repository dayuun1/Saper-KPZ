using Saper.Models;
using Saper.Models.DB;
using Saper.Models.DifficultyState;
using Saper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Saper.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ApplicationContext Database;
        private IWindowService _windowService;

        private readonly GameManager _gameManager;
        private readonly MenuState _menuState;

        public ObservableCollection<CellViewModel> Cells { get; }

        public ICommand CellClickCommand { get; private set; }
        public ICommand SafeClickCommand { get; private set; }
        public ICommand ToggleFlagCommand { get; private set; }
        public ICommand RestartGameCommand { get; private set; }
        public ICommand ChangeMenuViewCommand { get; private set; }
        public ICommand ShowSettingsCommand { get; private set; }
        public ICommand QuitCommand { get; private set; }

        public int Score => _gameManager.Score;
        public bool IsWin => _gameManager.IsWin;
        public int Rows => _gameManager.Rows;
        public int Columns => _gameManager.Columns;
        public bool CanUseSafeClick => _gameManager.SafeClick > 0;

        public bool MenuVisibility => _menuState.MenuVisibility;
        public bool[] MenuSettingVisibility => new bool[] { _menuState.MainMenuVisible, _menuState.SettingsVisible };

        private bool _isGameOver;
        public bool IsGameOver
        {
            get => _isGameOver;
            set { _isGameOver = value; OnPropertyChanged(nameof(IsGameOver)); }
        }

        private string _gameResultMessage = "";
        public string GameResultMessage
        {
            get => _gameResultMessage;
            set { _gameResultMessage = value; OnPropertyChanged(nameof(GameResultMessage)); }
        }

        public GameViewModel(GameManager gameManager, IWindowService windowService)
        {
            _gameManager = gameManager;
            _windowService = windowService;
            _menuState = new MenuState();

            Database = new ApplicationContext();
            Cells = new ObservableCollection<CellViewModel>();

            InitializeCommands();
            ConfigureGame();
            SubscribeToEvents();

            _gameManager.StartGame();
            InitializeCells();
        }

        private void InitializeCommands()
        {
            CellClickCommand = new CellClickCommand(this, _gameManager);
            ToggleFlagCommand = new ToggleFlagCommand(this, _gameManager);
            SafeClickCommand = new SafeClickCommand(this, _gameManager);
            RestartGameCommand = new RelayCommand(param => RestartGame());

            ChangeMenuViewCommand = new RelayCommand(obj =>
            {
                _menuState.ToggleMenu();
                NotifyMenuStateChanged();
            });

            ShowSettingsCommand = new RelayCommand(obj =>
            {
                _menuState.ShowSettings();
                NotifyMenuStateChanged();
            });

            QuitCommand = new RelayCommand(obj =>
            {
                Mediator.PageId = 2;
                User user = Database.Users.Find(Mediator.UserId);
                Database.SaveChanges();
                _windowService.OpenWindow();
            });
        }

        private void ConfigureGame()
        {
            _gameManager.Rows = Mediator.Rows;
            _gameManager.Columns = Mediator.Columns;

            IDifficultyState difficulty;
            switch (Mediator.Difficulty)
            {
                case "Easy":
                    difficulty = new BeginnerState(_gameManager);
                    break;
                case "Medium":
                    difficulty = new IntermediateState(_gameManager);
                    break;
                case "Hard":
                    difficulty = new HardState(_gameManager);
                    break;
                default:
                    difficulty = new IntermediateState(_gameManager);
                    break;
            }

            _gameManager.SetDifficulty(difficulty);
        }

        private void SubscribeToEvents()
        {
            _gameManager.GameEnded += OnGameEnded;
        }

        private void InitializeCells()
        {
            Cells.Clear();
            for (int i = 0; i < _gameManager.Rows; i++)
            {
                for (int j = 0; j < _gameManager.Columns; j++)
                {
                    var cell = _gameManager.Minefield.Cells[i, j];
                    var cellVM = new CellViewModel(i, j)
                    {
                        IsOpend = cell.IsOpend,
                        CellType = cell.CellType,
                        IsFlagged = cell.IsFlagged
                    };
                    Cells.Add(cellVM);
                }
            }
        }

        public void RestartGame()
        {
            _gameManager.RestartGame();
            InitializeCells();
            IsGameOver = false;
            GameResultMessage = "";

            NotifyGameStateChanged();
            NotifySafeClickChanged();
            (SafeClickCommand as SafeClickCommand)?.RaiseCanExecuteChanged();
        }

        private void OnGameEnded(bool isWin, int score, TimeSpan time)
        {
            IsGameOver = true;
            GameResultMessage = isWin
                ? $"Ви перемогли! Очки: {score}, Час: {time:mm\\:ss}"
                : $"Ви програли! Очки: {score}, Час: {time:mm\\:ss}";

            Database.GameResults.Add(new GameResult
            {
                Score = score,
                IsWin = isWin,
                TimeSpent = time,
                UserId = Mediator.UserId,
                Difficulty = Mediator.Difficulty
            });
        }

        public void NotifyGameStateChanged()
        {
            OnPropertyChanged(nameof(Rows));
            OnPropertyChanged(nameof(Columns));
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(IsGameOver));
            OnPropertyChanged(nameof(IsWin));
        }

        public void NotifyScoreChanged()
        {
            OnPropertyChanged(nameof(Score));
        }

        public void NotifySafeClickChanged()
        {
            OnPropertyChanged(nameof(CanUseSafeClick));
        }

        private void NotifyMenuStateChanged()
        {
            OnPropertyChanged(nameof(MenuSettingVisibility));
            OnPropertyChanged(nameof(MenuVisibility));
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
