using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DifficultyState
{
    public interface IDifficultyState
    {
        void GenerateMinefield();
        void UpdateScore(CellType cellType);
        void SetHints();
    }
}
