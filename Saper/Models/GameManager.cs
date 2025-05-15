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

            if (IsFirstMove(x, y))
            {
                RegenerateFieldUntilSafeCell(x, y);
            }

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
                    IsEnd = true;
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

                if (Minefield.CellsToOpen == 0)
                {
                    IsEnd = true;
                    IsWin = true;
                }
            }

            IsSafeClick = false;
        }

        private bool IsFirstMove(int x, int y) =>
            Minefield.CellsToOpen == Rows * Columns - Minefield.MineCount;

        private void RegenerateFieldUntilSafeCell(int x, int y)
        {
            while (Minefield.Cells[x, y].CellType != CellType.Zero)
            {
                DifficultyState.GenerateMinefield();
            }
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
        public void ShowBombHint()
        {
            if (ShowMine <= 0) return;

            MinusScore(20);
            ShowMine--;

            //позначити одну або декілька бомб як підказку
        }
        public void SafeClickHint()
        {
            MinusScore(5);
            if (SafeClick <= 0) return;

            IsSafeClick = true;
            SafeClick--;
        }
        public void ShowLowestMineCellHint()
        {
            MinusScore(20);
            if (ShowLowestMineCell <= 0) return;
            ShowLowestMineCell--;

            List<Cell> safeCandidates = new List<Cell>();
            Cell bestCell = null;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var cell = Minefield.Cells[i, j];

                    if (!cell.IsOpend && !cell.IsMine)
                    {
                        if (bestCell == null || cell.CellType < bestCell.CellType)
                        {
                            bestCell = cell;
                            safeCandidates.Clear();
                            safeCandidates.Add(cell);
                        }
                        else if (cell.CellType == bestCell.CellType)
                        {
                            safeCandidates.Add(cell);
                        }
                    }
                }
            }

            if (bestCell != null)
            {
                //виділити клітинку на полі для гравця
            }
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

    }
}
