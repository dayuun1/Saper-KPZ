
using Saper.Models.DifficultyState;

namespace Saper.Models
{
    public class ScoreManager
    {
        public int Score { get; private set; }

        public void UpdateForCell(CellType type, IDifficultyState state)
        {
            state.UpdateScore(type);
            Score = state.Score;
        }

        public void AddFlag()    => Score += 5;
        public void RemoveFlag() => Score = Math.Max(0, Score - 5);

        public void PenaltySafeClick()
        {
            Score = Math.Max(0, Score - 30);
        }
    }
}
