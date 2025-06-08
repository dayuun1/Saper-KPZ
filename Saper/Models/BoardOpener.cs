
namespace Saper.Models
{
    public class BoardOpener
    {
        private readonly Minefield _minefield;
        private readonly int _rows;
        private readonly int _cols;

        public BoardOpener(Minefield minefield, int rows, int cols)
        {
            _minefield = minefield;
            _rows = rows;
            _cols = cols;
        }

        private bool InBounds(int x, int y)
            => x >= 0 && y >= 0 && x < _rows && y < _cols;

        public void OpenEdgeCells()
        {
            for (int i = 0; i < _rows; i++)
            for (int j = 0; j < _cols; j++)
            {
                if (i == 0 || i == _rows - 1 || j == 0 || j == _cols - 1)
                {
                    var cell = _minefield.Cells[i, j];
                    if (!cell.IsOpend && !cell.IsMine)
                    {
                        cell.Open();
                        _minefield.CellsToOpen--;
                    }
                }
            }
        }

        public void OpenCell(int x, int y, GameManager manager)
        {
            if (!InBounds(x, y)) return;

            var cell = _minefield.Cells[x, y];
            if (cell.IsOpend) return;

            cell.Open();
            if (cell.IsMine)
            {
                if (manager.IsSafeClick)
                {
                    _minefield.MineCount--;
                }
                else
                {
                    manager.EndGame(false);
                    return;
                }
            }
            else
            {
                manager.ScoreManager.UpdateForCell(cell.CellType, manager.DifficultyState);
                _minefield.CellsToOpen--;
                if (cell.CellType == CellType.Zero)
                    OpenNeighbors(x, y);
            }

            if (_minefield.CellsToOpen == 0)
                manager.EndGame(true);

            manager.IsSafeClick = false;
        }

        private void OpenNeighbors(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                OpenCell(x + dx, y + dy, null);
            }
        }
    }
}
