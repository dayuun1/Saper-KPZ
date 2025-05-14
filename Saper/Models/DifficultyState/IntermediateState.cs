using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DifficultyState
{
    public class IntermediateState : IDifficultyState
    {
        private readonly GameManager _game;

        private const int MIN_MINES = 8;

        public IntermediateState(GameManager game)
        {
            _game = game;
        }

        public void GenerateMinefield()
        {
            int rows = _game.Rows;
            int cols = _game.Columns;
            int mines = Math.Max(MIN_MINES, rows * cols / 6); 

            _game.Minefield.InitializeField(rows, cols, mines);
        }

        public void UpdateScore(CellType cellType)
        {
            int score = cellType switch
            {
                CellType.Zero => 8,
                CellType.Five or CellType.Six => 20,
                CellType.Seven or CellType.Eight => 30,
                _ => 12
            };

            _game.Score += score;
        }

        public void SetHints()
        {
            _game.ShowLowestMineCell = 0;
            _game.ShowMine = 1;
            _game.SafeClick = 1;
        }
    }
}
