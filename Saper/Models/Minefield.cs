using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models
{
    public class Minefield
    {
        public Cell[,] Cells { get; set; }

        public int MineCount { get; set; }

        public int CellsToOpen { get; set; }

        public interface IMinePlacementStrategy
        {
            bool[] GenerateMines(int rows, int columns, int mineCount);
        }

        public class RandomMinePlacementStrategy : IMinePlacementStrategy
        {
            public bool[] GenerateMines(int rows, int columns, int mineCount)
            {
                int totalCells = rows * columns;
                bool[] minesArr = new bool[totalCells];

                List<int> validIndexList = new();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (!IsEdgeCell(i, j, rows, columns))
                        {
                            int index = i * columns + j;
                            validIndexList.Add(index);
                        }
                    }
                }

                int[] validIndices = validIndexList.ToArray();
                Random.Shared.Shuffle<int>(validIndices); 

                for (int i = 0; i < mineCount; i++)
                {
                    minesArr[validIndices[i]] = true;
                }

                return minesArr;
            }

            private bool IsEdgeCell(int row, int col, int rows, int columns)
            {
                return row == 0 || row == rows - 1 || col == 0 || col == columns - 1;
            }
        }

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
        public void InitializeField(int rows, int columns, int mineCount)
        {
            var strategy = new RandomMinePlacementStrategy();
            var generator = new FieldGenerator(strategy);

            Cells = generator.GenerateMinefield(rows, columns, mineCount, out int cellsToOpen);
            MineCount = mineCount;
            CellsToOpen = cellsToOpen;
        }
    }
}
