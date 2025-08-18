using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewFine.Navigation
{
    public sealed class NavigationService<TViewModel> : INavigationService<TViewModel>
        where TViewModel : class
    {
        private readonly NavigationStore _store;
        private readonly Func<TViewModel> _factory;

        public NavigationService(NavigationStore store, Func<TViewModel> factory)
        {
            _store = store;
            _factory = factory;
        }

        public void Navigate()
        {
            _store.CurrentViewModel = _factory();
        }
    }
}