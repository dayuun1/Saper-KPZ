using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Services
{
    public interface IWindowService
    {
        void OpenMainWindow();
        void OpenMenuWindow();
        void CloseCurrentWindow();
    }


}
