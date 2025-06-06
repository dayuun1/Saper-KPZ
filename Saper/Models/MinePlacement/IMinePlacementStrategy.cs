namespace Saper.Models.MinePlacement
{
    public interface IMinePlacementStrategy
    {
        bool[] GenerateMines(int rows, int columns, int mineCount);
    }
}