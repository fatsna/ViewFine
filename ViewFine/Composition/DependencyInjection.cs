// Composition/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using ViewFine.Navigation;
using ViewFine.Services;
using ViewFine.ViewModels;

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
            sc.AddTransient<ListViewModel>();
            sc.AddTransient<TrendsViewModel>();
            sc.AddTransient<CalculatorViewModel>();
            sc.AddTransient<UpdatesViewModel>();
            sc.AddTransient<FaqViewModel>();
            sc.AddSingleton<MainViewModel>();

            // Navigation services
            sc.AddTransient<INavigationService<ListViewModel>>(sp =>
                new NavigationService<ListViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<ListViewModel>()));

            sc.AddTransient<INavigationService<TrendsViewModel>>(sp =>
                new NavigationService<TrendsViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<TrendsViewModel>()));

            sc.AddTransient<INavigationService<CalculatorViewModel>>(sp =>
                new NavigationService<CalculatorViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<CalculatorViewModel>()));

            sc.AddTransient<INavigationService<UpdatesViewModel>>(sp =>
                new NavigationService<UpdatesViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<UpdatesViewModel>()));

            sc.AddTransient<INavigationService<FaqViewModel>>(sp =>
                new NavigationService<FaqViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<FaqViewModel>()));

            return sc;
        }
    }
}
