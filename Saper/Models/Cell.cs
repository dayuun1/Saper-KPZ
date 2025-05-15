using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models
{
    public class Cell
    {
        public bool IsMine { get; set; }

        public bool IsOpend { get; set; }
        public bool IsFlagged { get; set; }
        public CellType CellType { get; set; }

        public Cell(bool isMine, CellType cellType)
        {
            IsMine = isMine;
            IsOpend = false;
            IsFlagged = false;
            CellType = cellType;
        }

        public void Open()
        {
            IsOpend = true;
        }
        public void Flag()
        {
            IsFlagged = true;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
