using Saper.Models.DifficultyState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Saper.Models
{
    public class GameManager
    {
        public IDifficultyState DifficultyState { get; private set; }
        public Minefield Minefield { get; } = new();
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Score { get; set; }
        public bool IsEnd { get; set; }
        public bool IsWin { get; set; }
        public bool IsSafeClick { get; set; }
        public int ShowLowestMineCell { get; set; }
        public int ShowMine { get; set; }
        public int SafeClick { get; set; }

        public void SetDifficulty(IDifficultyState difficulty)
        {
            DifficultyState = difficulty;
            DifficultyState.SetHints();
        }
        public Stopwatch Stopwatch { get; private set; } = new Stopwatch();
        public void StartGame()
        {
            Stopwatch.Restart();
            DifficultyState.GenerateMinefield();
        }

        public void HandleCellOpen(CellType cellType)
        {
            Stopwatch.Stop();
            TimeSpan duration = Stopwatch.Elapsed;
            DifficultyState.UpdateScore(cellType);
        }
    }
}
