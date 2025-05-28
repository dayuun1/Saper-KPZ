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
            
        }
        public Stopwatch Stopwatch { get; private set; } = new Stopwatch();
        public void StartGame()
        {
            Stopwatch.Restart();
            DifficultyState.GenerateMinefield();
            DifficultyState.SetHints();
            OpenEdgeCells();
        }

        public void HandleCellOpen(CellType cellType)
        {
            Stopwatch.Stop();
            TimeSpan duration = Stopwatch.Elapsed;
            DifficultyState.UpdateScore(cellType);
        }
        private bool IsInsideBounds(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < Rows && y < Columns)
            {
                return true;
            }
            else { return false; }
        }

        public void OpenCell(int x, int y)
        {
            if (!IsInsideBounds(x, y)) return;

            Cell cell = Minefield.Cells[x, y];

            if (cell.IsOpend) return;

            cell.Open();

            if (cell.IsMine)
            {
                if (IsSafeClick)
                {
                    Minefield.MineCount--;
                }
                else
                {
                    EndGame(false);
                    return;  
                }
            }
            else
            {
                DifficultyState.UpdateScore(cell.CellType);
                Minefield.CellsToOpen--;

                if (cell.CellType == CellType.Zero)
                {
                    OpenSurroundingCells(x, y);
                }
            }

            if (Minefield.CellsToOpen == 0)
            {
                EndGame(true);
                return;
            }
            IsSafeClick = false;
        }

        private void OpenSurroundingCells(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if ((dx != 0 || dy != 0) && IsInsideBounds(nx, ny))
                    {
                        OpenCell(nx, ny);
                    }
                }
            }
        }
       
        public void SafeClickHint()
        {
            MinusScore(30);
            if (SafeClick <= 0) return;

            IsSafeClick = true;
            SafeClick--;
        }
       
        private void MinusScore(int amount)
        {
            Score -= amount;
            if (Score < 0) Score = 0;
        }
        public void ToggleFlag(int row, int col)
        {
            var cell = Minefield.Cells[row, col];
            if (cell.IsOpend) return;

            cell.IsFlagged = !cell.IsFlagged;

            if (cell.IsFlagged)
                Score += 5;
            else
                Score -= 5;
        }
        
        public void OpenEdgeCells()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i == 0 || i == Rows - 1 || j == 0 || j == Columns - 1)
                    {
                        var cell = Minefield.Cells[i, j];
                        if (!cell.IsOpend && !cell.IsMine)
                        {
                            cell.Open();
                            Minefield.CellsToOpen--;
                        }
                    }
                }
            }
        }
        public void RestartGame()
        {
            IsEnd = false;
            IsWin = false;
            Score = 0;
            StartGame();
        }

        public event Action<bool, int, TimeSpan>? GameEnded;

        public void EndGame(bool isWin)
        {
            IsEnd = true;
            IsWin = isWin;
            Stopwatch.Stop();
            GameEnded?.Invoke(isWin, Score, Stopwatch.Elapsed);
        }

    }
}
