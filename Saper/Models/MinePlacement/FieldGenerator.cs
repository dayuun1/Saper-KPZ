namespace Saper.Models.MinePlacement
{
    public class FieldGenerator
    {
        private readonly IMinePlacementStrategy _minePlacementStrategy;

        public FieldGenerator(IMinePlacementStrategy minePlacementStrategy)
        {
            _minePlacementStrategy = minePlacementStrategy;
        }

        public Cell[,] GenerateMinefield(int rows, int columns, int mineCount, out int cellsToOpen)
        {
            Cell[,] cells = new Cell[rows, columns];
            bool[] mineArr = _minePlacementStrategy.GenerateMines(rows, columns, mineCount);
            cellsToOpen = rows * columns - mineCount;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    bool isMine = mineArr[i * columns + j];
                    CellType cellType;

                    if (isMine)
                    {
                        cellType = CellType.Mine;
                    }
                    else
                    {
                        int mineNear = CountNearbyMines(mineArr, rows, columns, i, j);
                        cellType = (CellType)mineNear;
                    }

                    cells[i, j] = new Cell(isMine, cellType);
                }
            }

            return cells;
        }

        private int CountNearbyMines(bool[] minesArr, int rows, int columns, int i, int j)
        {
            int mineNear = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int ni = i + x;
                    int nj = j + y;

                    if ((x != 0 || y != 0) &&
                        ni >= 0 && nj >= 0 &&
                        ni < rows && nj < columns &&
                        minesArr[ni * columns + nj])
                    {
                        mineNear++;
                    }
                }
            }
            return mineNear;
        }
    }
}