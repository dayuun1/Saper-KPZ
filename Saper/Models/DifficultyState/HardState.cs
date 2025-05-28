using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DifficultyState
{
    public class HardState : IDifficultyState
    {
        private const int MIN_MINES = 15;

        public HardState(GameManager game) : base(game)
        {
            scorePerCellDictionary = new Dictionary<CellType, int>()
            {
                {CellType.Zero, 0 }, {CellType.One, 10},
                {CellType.Two, 10 }, {CellType.Three, 10},
                {CellType.Four, 10 }, {CellType.Five, 15},
                {CellType.Six, 15}, {CellType.Seven, 25},
                {CellType.Eight, 25 }
            };
        }

        public override void GenerateMinefield()
        {
            int rows = _game.Rows;
            int cols = _game.Columns;
            int percent = rows * cols / 4;
            int mines = Math.Max(MIN_MINES, percent);

            _game.Minefield.InitializeField(rows, cols, mines);
        }

        public override void SetHints()
        {
            _game.SafeClick = 0;
        }
    }
}
