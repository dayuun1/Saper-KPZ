using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DifficultyState
{
    public abstract class IDifficultyState
    {
        public IDifficultyState(GameManager game)
        {
            _game = game;
        }

        protected Dictionary<CellType, int> scorePerCellDictionary = new Dictionary<CellType, int>();
        protected readonly GameManager _game;

        public abstract void GenerateMinefield();
        public virtual void UpdateScore(CellType cellType)
        {
            _game.Score += scorePerCellDictionary[cellType];
        }
        public abstract void SetHints();
    }
}
