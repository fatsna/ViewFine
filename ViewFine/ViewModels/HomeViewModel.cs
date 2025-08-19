using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ViewFine.Models;
using ViewFine.Navigation;
using System;

namespace ViewFine.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly NavigationStore _store;
        private readonly Func<PenaltyCategory, ListViewModel> _listFactory;
        private readonly INavigationService<TrendsViewModel> _trendsNav;
        private readonly INavigationService<CalculatorViewModel> _calcNav;
        private readonly INavigationService<UpdatesViewModel> _updatesNav;
        private readonly INavigationService<FaqViewModel> _faqNav;

        public HomeViewModel(
            NavigationStore store,
            Func<PenaltyCategory, ListViewModel> listFactory,
            INavigationService<TrendsViewModel> trendsNav,
            INavigationService<CalculatorViewModel> calcNav,
            INavigationService<UpdatesViewModel> updatesNav,
            INavigationService<FaqViewModel> faqNav)
        {
            _store = store;
            _listFactory = listFactory;
            _trendsNav = trendsNav;
            _calcNav = calcNav;
            _updatesNav = updatesNav;
            _faqNav = faqNav;
        }

        // ✅ 전체 목록 제거 → 두 카테고리만 남김
        //[RelayCommand] void OpenCancel() => _store.CurrentViewModel = _listFactory(PenaltyCategory.Cancel);
        //[RelayCommand] void OpenSuspend() => _store.CurrentViewModel = _listFactory(PenaltyCategory.SuspendA);
        [RelayCommand] void OpenCancel() => _store.NavigateTo(_listFactory(PenaltyCategory.Cancel));
        [RelayCommand] void OpenSuspend() => _store.NavigateTo(_listFactory(PenaltyCategory.SuspendA));

        [RelayCommand] void OpenTrends() => _trendsNav.Navigate();
        [RelayCommand] void OpenCalc() => _calcNav.Navigate();
        [RelayCommand] void OpenUpdates() => _updatesNav.Navigate();
        [RelayCommand] void OpenFaq() => _faqNav.Navigate();
    }
}
