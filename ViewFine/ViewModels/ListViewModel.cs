using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ViewFine.Core;
using ViewFine.Services;

namespace ViewFine.ViewModels
{
    public partial class ListViewModel : ObservableObject
    {
        private readonly IDbService _db;

        public ObservableCollection<PenaltyRow> Items { get; } = new();
        public ICollectionView View { get; }

        [ObservableProperty] private string? _searchText;

        public ListViewModel(IDbService db)
        {
            _db = db;
            View = System.Windows.Data.CollectionViewSource.GetDefaultView(Items);
            View.Filter = Filter;
            _ = LoadAsync();
        }

        private bool Filter(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            var it = (PenaltyRow)obj;
            var q = SearchText!.Trim();
            return (it.item_no?.Contains(q) ?? false)
                || (it.label?.Contains(q) ?? false)
                || (it.law?.Contains(q) ?? false)
                || (it.category?.Contains(q) ?? false);
        }

        partial void OnSearchTextChanged(string? value) => View.Refresh();

        [RelayCommand]
        public async Task LoadAsync()
        {
            Items.Clear();
            var rows = await _db.GetMergedPenaltiesAsync(); // 서버측 검색 원하면 param 활용
            foreach (var r in rows) Items.Add(r);
            View.Refresh();
        }
    }
}
