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
        public ObservableCollection<CellViewModel> Cells { get; }

        public ICommand CellClickCommand { get; }
        public ICommand SafeClickCommand { get; }
        public ICommand ToggleFlagCommand { get; }
        public ICommand RestartGameCommand { get; }
        public ICommand ChangeMenuViewCommand { get; set; }
        public ICommand ShowSettingsCommand { get; set; }
        public ICommand QuitCommand { get; set; }


        public int Score => _gameManager.Score;
        public bool IsWin => _gameManager.IsWin;
        public int Rows => _gameManager.Rows;
        public int Columns => _gameManager.Columns;
        public bool CanUseSafeClick => _gameManager.SafeClick > 0;
        public bool[] MenuSettingVisibility { get; set; }
        public bool MenuVisibility { get; set; }
        public GameViewModel(GameManager gameManager, IWindowService windowService)
        {
            _gameManager = gameManager;
            gameManager.GameEnded += OnGameEnded;
            Database = new ApplicationContext();
            _windowService = windowService;
            MenuVisibility = false;
            MenuSettingVisibility = [true, false];
            Cells = new ObservableCollection<CellViewModel>();

            CellClickCommand = new RelayCommand(param =>
            {
                if (param is CellViewModel cell)
                {
                    if (cell.IsFlagged)
                        return;

                    _gameManager.OpenCell(cell.Row, cell.Column);
                    var actualCell = _gameManager.Minefield.Cells[cell.Row, cell.Column];
                    cell.IsOpend = actualCell.IsOpend;
                    cell.CellType = actualCell.CellType;

                    OnPropertyChanged(nameof(Rows));
                    OnPropertyChanged(nameof(Columns));
                    OnPropertyChanged(nameof(Score));
                    OnPropertyChanged(nameof(IsGameOver));
                    OnPropertyChanged(nameof(IsWin));
                }
            });

            ToggleFlagCommand = new RelayCommand(param =>
            {
                if (param is CellViewModel cell && !cell.IsOpend)
                {
                    _gameManager.ToggleFlag(cell.Row, cell.Column);
                    var actualCell = _gameManager.Minefield.Cells[cell.Row, cell.Column];
                    cell.IsFlagged = actualCell.IsFlagged;

                    OnPropertyChanged(nameof(Score));
                }
            });

            ChangeMenuViewCommand = new RelayCommand(obj =>
            {
                if (MenuVisibility && MenuSettingVisibility[0])
                {
                    MenuVisibility = false;
                }
                else
                {
                    MenuVisibility = true;
                }
                MenuSettingVisibility[0] = true;
                MenuSettingVisibility[1] = false;
                OnPropertyChanged(nameof(MenuSettingVisibility));
                OnPropertyChanged(nameof(MenuVisibility));
            });

            ShowSettingsCommand = new RelayCommand(obj =>
            {
                MenuSettingVisibility[0] = false;
                MenuSettingVisibility[1] = true;
                OnPropertyChanged(nameof(MenuSettingVisibility));
            });

            QuitCommand = new RelayCommand(obj =>
            {
                Mediator.PageId = 2;
                User user = Database.Users.Find(Mediator.UserId);
                Mediator.IsMuted = IsMuted;
                user.IsSoundMuted = IsMuted;
                Database.SaveChanges();
                _windowService.OpenWindow();
            });

            SafeClickCommand = new RelayCommand(
    param =>
    {
        _gameManager.SafeClickHint();
        OnPropertyChanged(nameof(CanUseSafeClick));
        (SafeClickCommand as RelayCommand)?.RaiseCanExecuteChanged();
        OnPropertyChanged(nameof(Score));
    },
    param => CanUseSafeClick
);
            RestartGameCommand = new RelayCommand(param => RestartGame());
            gameManager.Rows = Mediator.Rows;
            _gameManager.Columns = Mediator.Columns;

            switch (Mediator.Difficulty)
            {
                case "Easy": _gameManager.SetDifficulty(new BeginnerState(_gameManager)); break;
                case "Medium": _gameManager.SetDifficulty(new IntermediateState(_gameManager)); break;
                case "Hard": _gameManager.SetDifficulty(new HardState(_gameManager)); break;
                default: _gameManager.SetDifficulty(new IntermediateState(_gameManager)); break;
            }
            _gameManager.StartGame(); 

            InitializeCells();
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                if (_isMuted)
                {
                    MutedIcon = "../Images/notmuted.jpg";
                }
                else
                {
                    MutedIcon = "../Images/muted.jpg";
                }
                _isMuted = value;
                OnPropertyChanged(nameof(IsMuted));
            }
        }

        private string _mutedIcon = "../Images/notmuted";
        public string MutedIcon
        {
            get => _mutedIcon;
            set
            {
                _mutedIcon = value;
                OnPropertyChanged(nameof(MutedIcon));
            }
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
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(IsGameOver));
            OnPropertyChanged(nameof(IsWin));
            OnPropertyChanged(nameof(CanUseSafeClick));
            (SafeClickCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
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
        private void OnGameEnded(bool isWin, int score, TimeSpan time)
        {
            IsGameOver = true;
            GameResultMessage = isWin
                ? $"Ви перемогли! Очки: {score}, Час: {time:mm\\:ss}"
                : $"Ви програли! Очки: {score}, Час: {time:mm\\:ss}";
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
