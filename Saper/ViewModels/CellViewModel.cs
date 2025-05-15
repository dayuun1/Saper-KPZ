using Saper.Models;
using Saper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IWindowService _windowService;

        public int Row { get; }
        public int Column { get; }

        private CellType _cellType = CellType.Zero;
        private bool _isFlagged;
        private bool _isOpend;
        public CellType CellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
                OnPropertyChanged(nameof(CellType));
            }
        }

        public bool IsFlagged
        {
            get { return _isFlagged; }
            set
            {
                if (_isFlagged != value)
                {
                    _isFlagged = value;
                    OnPropertyChanged(nameof(IsFlagged));
                }
            }
        }

        public bool IsOpend
        {
            get { return _isOpend; }
            set
            {
                if (_isOpend != value)
                {
                    _isOpend = value;
                    OnPropertyChanged(nameof(IsOpend));
                }
            }
        }

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                _isHighlighted = value;
                OnPropertyChanged(nameof(IsHighlighted));
            }
        }

        public CellViewModel(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
