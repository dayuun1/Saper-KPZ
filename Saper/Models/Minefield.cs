using Saper.Models.MinePlacement;

namespace Saper.Models
{
    public class Minefield
    {
        public Cell[,] Cells { get; set; }
        public int MineCount { get; set; }
        public int CellsToOpen { get; set; }

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