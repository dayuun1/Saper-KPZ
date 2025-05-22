using Saper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Saper.ViewModels
{
    public class SafeClickCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly GameManager _gameManager;

        public SafeClickCommand(GameViewModel viewModel, GameManager gameManager)
        {
            _viewModel = viewModel;
            _gameManager = gameManager;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _gameManager.SafeClick > 0;

        public void Execute(object parameter)
        {
            _gameManager.SafeClickHint();
            _viewModel.NotifyScoreChanged();
            _viewModel.NotifySafeClickChanged();
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
