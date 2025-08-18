// App.xaml.cs
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using ViewFine.Composition;
using ViewFine.ViewModels;
using ViewFine.Views;

namespace ViewFine
{
    public partial class App : Application
    {
        public static IServiceProvider Services = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Services = new ServiceCollection()
                .AddViewFine()
                .BuildServiceProvider();

            var vm = Services.GetRequiredService<ViewFine.ViewModels.MainViewModel>();
            var win = new ViewFine.Views.MainWindow { DataContext = vm }; // Views 폴더에 MainWindow.xaml이 있다면
            win.Show();
        }
    }
}
