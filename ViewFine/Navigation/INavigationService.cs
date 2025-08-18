using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewFine.Navigation
{
    public interface INavigationService<TViewModel> where TViewModel : class
    {
        void Navigate();
    }
}

