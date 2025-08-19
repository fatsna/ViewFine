using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ViewFine.Navigation
{
    public partial class NavigationStore : ObservableObject
    {
        private readonly Stack<object> _navigationHistory = new();

        [ObservableProperty]
        private object? _currentViewModel;

        [ObservableProperty]
        private bool _canGoBack;

        partial void OnCurrentViewModelChanged(object? value)
        {
            UpdateCanGoBack();
        }

        public void NavigateTo(object viewModel, bool addToHistory = true)
        {
            // 현재 ViewModel을 히스토리에 추가 (단, 같은 타입이 아닌 경우만)
            if (addToHistory && CurrentViewModel != null &&
                CurrentViewModel.GetType() != viewModel.GetType())
            {
                _navigationHistory.Push(CurrentViewModel);
            }

            CurrentViewModel = viewModel;
        }

        public void NavigateBack()
        {
            if (_navigationHistory.Count > 0)
            {
                var previousViewModel = _navigationHistory.Pop();
                NavigateTo(previousViewModel, addToHistory: false); // 뒤로가기는 히스토리에 추가하지 않음
            }
        }

        private void UpdateCanGoBack()
        {
            CanGoBack = _navigationHistory.Count > 0;
        }

        public void ClearHistory()
        {
            _navigationHistory.Clear();
            UpdateCanGoBack();
        }
    }
}