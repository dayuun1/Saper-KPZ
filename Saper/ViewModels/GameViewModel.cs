using Saper.Models;
using Saper.Models.DifficultyState;
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

        private readonly GameManager _gameManager;
        public ObservableCollection<CellViewModel> Cells { get; }

        public ICommand CellClickCommand { get; }
        public ICommand ShowHintCommand { get; }
        public ICommand ShowBombCommand { get; }
        public ICommand SafeClickCommand { get; }
        public ICommand ToggleFlagCommand { get; }

        public int Score => _gameManager.Score;
        public bool IsWin => _gameManager.IsWin;
        public int Rows => _gameManager.Rows;
        public int Columns => _gameManager.Columns;

        public GameViewModel(GameManager gameManager)
        {
            _gameManager = gameManager;
            gameManager.GameEnded += OnGameEnded;
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

            SafeClickCommand = new RelayCommand(param =>
            {
                _gameManager.SafeClickHint();
            });
            _gameManager.Rows = 10;
            _gameManager.Columns = 10;
            _gameManager.SetDifficulty(new IntermediateState(_gameManager));
            _gameManager.StartGame(); 

            InitializeCells();
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
