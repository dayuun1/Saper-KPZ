using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DifficultyState
{
    public class HardState : IDifficultyState
    {
        private readonly GameManager _game;

        private const int MIN_MINES = 15;

        public HardState(GameManager game)
        {
            _game = game;
        }

        public void GenerateMinefield()
        {
            int rows = _game.Rows;
            int cols = _game.Columns;
            int mines = Math.Max(MIN_MINES, rows * cols / 4); 

            _game.Minefield.InitializeField(rows, cols, mines);
        }

        public void UpdateScore(CellType cellType)
        {
            int score = cellType switch
            {
                CellType.Zero => 0,
                CellType.Five or CellType.Six => 15,
                CellType.Seven or CellType.Eight => 25,
                _ => 10
            };

            _game.Score += score;
        }

        public void SetHints()
        {
            _game.ShowLowestMineCell = 0;
            _game.ShowMine = 0;
            _game.SafeClick = 0;
        }
    }
}
