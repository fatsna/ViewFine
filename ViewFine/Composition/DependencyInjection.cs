// Composition/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using ViewFine.Navigation;
using ViewFine.Services;
using ViewFine.ViewModels;
using ViewFine.Models;

namespace ViewFine.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddViewFine(this IServiceCollection sc)
        {
            // Core
            sc.AddSingleton<IDbService>(sp => new DbService());
            sc.AddSingleton<NavigationStore>();

            // ViewModels
            sc.AddTransient<HomeViewModel>();
            sc.AddTransient<ListViewModel>();
            sc.AddTransient<TrendsViewModel>();
            sc.AddTransient<CalculatorViewModel>();
            sc.AddTransient<UpdatesViewModel>();
            sc.AddTransient<FaqViewModel>();
            sc.AddSingleton<MainViewModel>();

            sc.AddTransient<Func<PenaltyCategory, ListViewModel>>(sp => cat =>
                new ListViewModel(sp.GetRequiredService<IDbService>(), cat));

            // 제네릭 네비게이션 서비스 등록 (모든 ViewModel에 대해 자동으로 적용)
            sc.AddTransient(typeof(INavigationService<>), typeof(NavigationService<>));

            return sc;
        }
    }
}