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
        private readonly GameManager _game;

        public BeginnerState(GameManager game)
        {
            _game = game;
        }

        private const int MIN_MINES = 3;

        public void GenerateMinefield()
        {
            int rows = _game.Rows;
            int cols = _game.Columns;
            int mines = Math.Max(MIN_MINES, rows * cols / 10);

            _game.Minefield.InitializeField(rows, cols, mines);
        }

        public void UpdateScore(CellType cellType)
        {
            int score = cellType switch
            {
                CellType.Zero => 0,
                CellType.Five or CellType.Six => 25,
                CellType.Seven or CellType.Eight => 35,
                _ => 15
            };

            _game.Score += score;
        }

        public void SetHints()
        {
            _game.ShowLowestMineCell = 2;
            _game.ShowMine = 1;
            _game.SafeClick = 1;
        }
    }
}
