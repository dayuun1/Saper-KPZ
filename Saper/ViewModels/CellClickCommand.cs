using Saper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Saper.ViewModels
{
    public class CellClickCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly GameManager _gameManager;

        public CellClickCommand(GameViewModel viewModel, GameManager gameManager)
        {
            _viewModel = viewModel;
            _gameManager = gameManager;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is not CellViewModel cell || cell.IsFlagged)
                return;

            _gameManager.OpenCell(cell.Row, cell.Column);
            var actualCell = _gameManager.Minefield.Cells[cell.Row, cell.Column];

            cell.IsOpend = actualCell.IsOpend;
            cell.CellType = actualCell.CellType;

            _viewModel.NotifyGameStateChanged();
        }
    }
}