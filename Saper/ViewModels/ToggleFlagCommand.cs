using Saper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Saper.ViewModels
{
    public class ToggleFlagCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly GameManager _gameManager;

        public ToggleFlagCommand(GameViewModel viewModel, GameManager gameManager)
        {
            _viewModel = viewModel;
            _gameManager = gameManager;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) =>
            parameter is CellViewModel cell && !cell.IsOpend;

        public void Execute(object parameter)
        {
            if (parameter is not CellViewModel cell || cell.IsOpend)
                return;

            _gameManager.ToggleFlag(cell.Row, cell.Column);
            var actualCell = _gameManager.Minefield.Cells[cell.Row, cell.Column];
            cell.IsFlagged = actualCell.IsFlagged;

            _viewModel.NotifyScoreChanged();
        }
    }
}
