using Saper.Models;
using Saper.Models.DB;
using Saper.Models.DifficultyState;
using Saper.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Saper.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly GameManager _gameManager;
        private readonly IGameDatabaseService _databaseService;
        private readonly IMenuStateService _menuStateService;
        private readonly IWindowService _windowService;
        private readonly IWindowService _mainWindowService;  // <-- Додано, якщо потрібно
        private readonly GameConfiguration _gameConfiguration;

        public ObservableCollection<CellViewModel> Cells { get; }

        public ICommand CellClickCommand { get; private set; }
        public ICommand SafeClickCommand { get; private set; }
        public ICommand ToggleFlagCommand { get; private set; }
        public ICommand RestartGameCommand { get; private set; }
        public ICommand ChangeMenuViewCommand { get; private set; }
        public ICommand ShowSettingsCommand { get; private set; }
        public ICommand QuitCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }  // Якщо потрібно

        public int Score => _gameManager.Score;
        public bool IsWin => _gameManager.IsWin;
        public int Rows => _gameManager.Rows;
        public int Columns => _gameManager.Columns;
        public bool CanUseSafeClick => _gameManager.SafeClick > 0;

        public bool MenuVisibility => _menuStateService.MenuVisibility;
        public bool[] MenuSettingVisibility => new[]
        {
            _menuStateService.MainMenuVisible,
            _menuStateService.SettingsVisible
        };

        private bool _isGameOver;
        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                _isGameOver = value;
                OnPropertyChanged(nameof(IsGameOver));
            }
        }

        private string _gameResultMessage = "";
        public string GameResultMessage
        {
            get => _gameResultMessage;
            set
            {
                _gameResultMessage = value;
                OnPropertyChanged(nameof(GameResultMessage));
            }
        }

        public GameViewModel(
            GameManager gameManager,
            IWindowService windowService,
            IWindowService mainWindowService,   // якщо потрібен другий сервіс
            IGameDatabaseService databaseService,
            IMenuStateService menuStateService,
            GameConfiguration gameConfiguration)
        {
            _gameManager = gameManager;
            _windowService = windowService;
            _mainWindowService = mainWindowService;
            _databaseService = databaseService;
            _menuStateService = menuStateService;
            _gameConfiguration = gameConfiguration;

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
            RestartGameCommand = new RelayCommand(_ => RestartGame());

            ChangeMenuViewCommand = new RelayCommand(_ =>
            {
                _menuStateService.ToggleMenu();
                NotifyMenuStateChanged();
            });

            ShowSettingsCommand = new RelayCommand(_ =>
            {
                _menuStateService.ShowSettings();
                NotifyMenuStateChanged();
            });

            QuitCommand = new RelayCommand(_ =>
            {
                _databaseService.SaveUserSession(_gameConfiguration.UserId);

                _mainWindowService.OpenMenuWindow();
                _mainWindowService.CloseCurrentWindow();
            });

            ExitCommand = new RelayCommand(_ =>
            {
                _mainWindowService.OpenMenuWindow();
                _mainWindowService.CloseCurrentWindow();
            });
        }

        private void ConfigureGame()
        {
            _gameManager.Rows = _gameConfiguration.Rows;
            _gameManager.Columns = _gameConfiguration.Columns;

            IDifficultyState difficulty = _gameConfiguration.Difficulty switch
            {
                "Easy" => new BeginnerState(_gameManager),
                "Medium" => new IntermediateState(_gameManager),
                "Hard" => new HardState(_gameManager),
                _ => new IntermediateState(_gameManager)
            };

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
                    Cells.Add(new CellViewModel(i, j)
                    {
                        IsOpend = cell.IsOpend,
                        CellType = cell.CellType,
                        IsFlagged = cell.IsFlagged
                    });
                }
            }
        }

        public void RestartGame()
        {
            _gameManager.RestartGame();
            InitializeCells();

            IsGameOver = false;
            GameResultMessage = string.Empty;

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

            _databaseService.SaveGameResult(new GameResult
            {
                Score = score,
                IsWin = isWin,
                TimeSpent = time,
                UserId = _gameConfiguration.UserId,
                Difficulty = _gameConfiguration.Difficulty
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

        public void NotifyScoreChanged() => OnPropertyChanged(nameof(Score));

        public void NotifySafeClickChanged() => OnPropertyChanged(nameof(CanUseSafeClick));

        private void NotifyMenuStateChanged()
        {
            OnPropertyChanged(nameof(MenuSettingVisibility));
            OnPropertyChanged(nameof(MenuVisibility));
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
