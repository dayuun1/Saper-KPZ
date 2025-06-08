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
            bool[] minesArray = _minePlacementStrategy.GenerateMines(rows, columns, mineCount);
            cellsToOpen = rows * columns - mineCount;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cells[i, j] = CreateCell(i, j, minesArray, rows, columns);
                }
            }

            return cells;
        }

        private Cell CreateCell(int row, int column, bool[] minesArray, int rows, int columns)
        {
			bool isMine = minesArray[GetIndex(row, column, columns)];
			CellType cellType = isMine 
                ? CellType.Mine 
                : (CellType) CountNearbyMines(minesArray, rows, columns, row, column);
            return new Cell(isMine, cellType);

		}

		private int GetIndex(int row, int column, int columns) => row * columns + column;

        private int CountNearbyMines(bool[] minesArr, int rows, int columns, int i, int j)
        {
            int nearbyMinesCount = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int ni = i + x;
                    int nj = j + y;

                    if ((x != 0 || y != 0) &&
                        ni >= 0 && nj >= 0 &&
                        ni < rows && nj < columns &&
                        minesArr[GetIndex(ni, nj, columns)])
                    {
						nearbyMinesCount++;
                    }
                }
            }
            return nearbyMinesCount;
        }
    }
}