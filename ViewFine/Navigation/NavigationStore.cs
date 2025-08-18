using CommunityToolkit.Mvvm.ComponentModel;

namespace ViewFine.Navigation
{
    public partial class NavigationStore : ObservableObject
    {
        [ObservableProperty] private object? _currentViewModel;
    }
}
