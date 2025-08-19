// Navigation/NavigationService.cs
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ViewFine.Navigation
{
    public sealed class NavigationService<TViewModel> : INavigationService<TViewModel>
        where TViewModel : class
    {
        private readonly NavigationStore _store;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(NavigationStore store, IServiceProvider serviceProvider)
        {
            _store = store;
            _serviceProvider = serviceProvider;
        }

        public void Navigate()
        {
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            _store.NavigateTo(viewModel); // 히스토리에 추가하며 네비게이션
        }
    }
}