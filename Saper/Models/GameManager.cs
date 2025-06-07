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
        public Minefield Minefield { get; private set; }
        public GameTimer Timer      { get; private set; }
        public ScoreManager ScoreManager { get; private set; }
        public BoardOpener BoardOpener   { get; private set; }

        public int Rows   { get; set; }
        public int Columns{ get; set; }
        public bool IsSafeClick { get; set; }

        public event Action<bool, int, TimeSpan>? GameEnded;

        public GameManager(IDifficultyState difficulty, int rows, int columns)
        {
            DifficultyState = difficulty;
            Rows = rows;
            Columns = columns;

            Minefield    = new Minefield();
            Timer        = new GameTimer();
            ScoreManager = new ScoreManager();
            BoardOpener  = new BoardOpener(Minefield, Rows, Columns);
        }

        public void StartGame()
        {
            Timer.Start();
            DifficultyState.GenerateMinefield();
            DifficultyState.SetHints();
            BoardOpener.OpenEdgeCells();
        }

        public void OpenCell(int x, int y)
        {
            BoardOpener.OpenCell(x, y, this);
        }

        public void SafeClickHint()
        {
            ScoreManager.PenaltySafeClick();
            if (ScoreManager.Score <= 0) return;
            IsSafeClick = true;
        }

        public void ToggleFlag(int row, int col)
        {
            var cell = Minefield.Cells[row, col];
            if (cell.IsOpend) return;

            cell.IsFlagged = !cell.IsFlagged;
            if (cell.IsFlagged) ScoreManager.AddFlag();
            else                ScoreManager.RemoveFlag();
        }

        public void RestartGame()
        {
            IsSafeClick = false;
            Minefield = new Minefield();
            ScoreManager = new ScoreManager();
            StartGame();
        }

        public void EndGame(bool isWin)
        {
            Timer.Stop();
            GameEnded?.Invoke(isWin, ScoreManager.Score, Timer.Elapsed);
        }

    }
}
