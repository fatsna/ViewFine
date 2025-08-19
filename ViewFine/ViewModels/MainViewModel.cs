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

        [ObservableProperty] private object? _currentViewModel;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BackCommand))]
         private bool _canGoBack;

        public MainViewModel(
            NavigationStore store,
            INavigationService<HomeViewModel> homeNav,
            INavigationService<ListViewModel> listNav,
            INavigationService<TrendsViewModel> trendsNav,
            INavigationService<CalculatorViewModel> calcNav,
            INavigationService<UpdatesViewModel> updatesNav,
            INavigationService<FaqViewModel> faqNav)
        {
            _store = store;
            _listNav = listNav;
            _trendsNav = trendsNav;
            _calcNav = calcNav;
            _updatesNav = updatesNav;
            _faqNav = faqNav;

            // NavigationStore와 동기화
            _store.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
                    CurrentViewModel = _store.CurrentViewModel;
                else if (e.PropertyName == nameof(NavigationStore.CanGoBack))
                    CanGoBack = _store.CanGoBack;
            };

            homeNav.Navigate(); // 초기 화면
            _store.ClearHistory(); // 초기 화면은 히스토리에서 제거

            // 초기값 설정
            CurrentViewModel = _store.CurrentViewModel;
            CanGoBack = _store.CanGoBack;
        }

        // 기존 네비게이션 명령들
        [RelayCommand] void NavigateList() => _listNav.Navigate();
        [RelayCommand] void NavigateTrends() => _trendsNav.Navigate();
        [RelayCommand] void NavigateCalc() => _calcNav.Navigate();
        [RelayCommand] void NavigateUpdates() => _updatesNav.Navigate();
        [RelayCommand] void NavigateFaq() => _faqNav.Navigate();


        // 뒤로가기 명령
        [RelayCommand(CanExecute = nameof(CanGoBack))]
        void Back() => _store.NavigateBack();

    }
}