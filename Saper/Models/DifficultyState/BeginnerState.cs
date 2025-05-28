using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saper.Models;

namespace Saper.Models.DifficultyState
{
    public class BeginnerState : IDifficultyState
    {
        public BeginnerState(GameManager game) : base(game)
        {
            scorePerCellDictionary = new Dictionary<CellType, int>()
            {
                {CellType.Zero, 0 }, {CellType.One, 15},
                {CellType.Two, 15 }, {CellType.Three, 15},
                {CellType.Four, 15 }, {CellType.Five, 25},
                {CellType.Six, 25}, {CellType.Seven, 35},
                {CellType.Eight, 35 }
            };
        }

        private const int MIN_MINES = 3;

        public override void GenerateMinefield()
        {
            int rows = _game.Rows;
            int cols = _game.Columns;
            int percent = rows * cols / 10;
            int mines = Math.Max(MIN_MINES, percent);

            _game.Minefield.InitializeField(rows, cols, mines);
        }

        public override void SetHints()
        {
            _game.SafeClick = 5;
        }
    }
}
