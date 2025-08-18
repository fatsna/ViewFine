using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ViewFine.Navigation;

namespace ViewFine.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly NavigationStore _store;
        private readonly INavigationService<ListViewModel> _listNav;
        private readonly INavigationService<TrendsViewModel> _trendsNav;
        private readonly INavigationService<CalculatorViewModel> _calcNav;
        private readonly INavigationService<UpdatesViewModel> _updatesNav;
        private readonly INavigationService<FaqViewModel> _faqNav;

        public object? CurrentViewModel => _store.CurrentViewModel;

        public MainViewModel(
            NavigationStore store,
            INavigationService<ListViewModel> listNav,
            INavigationService<TrendsViewModel> trendsNav,
            INavigationService<CalculatorViewModel> calcNav,
            INavigationService<UpdatesViewModel> updatesNav,
            INavigationService<FaqViewModel> faqNav)
        {
            _store = store;
            _listNav = listNav; _trendsNav = trendsNav; _calcNav = calcNav; _updatesNav = updatesNav; _faqNav = faqNav;
            _store.PropertyChanged += (_, __) => OnPropertyChanged(nameof(CurrentViewModel));
            _listNav.Navigate(); // 초기 화면
        }

        [RelayCommand] void NavigateList() => _listNav.Navigate();
        [RelayCommand] void NavigateTrends() => _trendsNav.Navigate();
        [RelayCommand] void NavigateCalc() => _calcNav.Navigate();
        [RelayCommand] void NavigateUpdates() => _updatesNav.Navigate();
        [RelayCommand] void NavigateFaq() => _faqNav.Navigate();
    }
}
