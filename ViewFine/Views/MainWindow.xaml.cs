using System;
using System.Data;
using System.IO;
using System.Windows;
using ViewFine.Core;

namespace ViewFine
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "law_portable.db");
            if (!File.Exists(dbPath))
            {
                MessageBox.Show($"DB 파일이 없습니다:\n{dbPath}",
                    "DB Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await using var db = new DbService(dbPath);
                var (cancelCount, suspendCount) = await db.GetCountsAsync();
                TxtStatus.Text = $"취소:{cancelCount} / 정지(가):{suspendCount}";

                DataTable cancel = await db.GetCancelAsync();
                DataTable suspend = await db.GetSuspendAAsync();

                GridCancel.ItemsSource = cancel.DefaultView;
                GridSuspend.ItemsSource = suspend.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
