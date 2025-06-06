namespace Saper.Models.MinePlacement
{
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
}