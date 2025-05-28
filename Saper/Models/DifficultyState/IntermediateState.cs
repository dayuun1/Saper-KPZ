using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DifficultyState
{
    public class IntermediateState : IDifficultyState
    {
        private const int MIN_MINES = 8;

        public IntermediateState(GameManager game) : base(game) 
        {
            scorePerCellDictionary = new Dictionary<CellType, int>()
            {
                {CellType.Zero, 0 }, {CellType.One, 12},
                {CellType.Two, 12 }, {CellType.Three, 12},
                {CellType.Four, 12 }, {CellType.Five, 20},
                {CellType.Six, 20}, {CellType.Seven, 30},
                {CellType.Eight, 30 }
            };
        }

        public override void GenerateMinefield()
        {
            int rows = _game.Rows;
            int cols = _game.Columns;
            int percent = rows * cols / 6;
            int mines = Math.Max(MIN_MINES, percent); 

            _game.Minefield.InitializeField(rows, cols, mines);
        }

        public override void SetHints()
        {
            _game.SafeClick = 2;
        }
    }
}
