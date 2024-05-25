using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Markup.Xaml;

namespace newjeans_avalonia
{
    public partial class ThirdWindow : Window
    {
        private AppState _appState;

        public ThirdWindow(AppState appState)
        {
            InitializeComponent();
            _appState = appState;

            // if (_appState.CurrentImage != null)
            // {
            //     //
            // }

            BindDummyData();
            
            this.FindControl<Button>("BackButton")!.Click += OnBackButtonClick;
            this.FindControl<Button>("RetryButton")!.Click += OnRetryButtonClick;
        }

        private void BindDummyData()
        {
            NamaText.Text = "John Doe";
            NikText.Text = "1234567890";
            TempatLahirText.Text = "Jakarta";
            TanggalLahirText.Text = "01 Januari 2000";
            JenisKelaminText.Text = "Laki-laki";
            GolonganDarahText.Text = "A+";
            AlamatText.Text = "Jl. Raya No. 123";
            AgamaText.Text = "Islam";
            StatusPerkawinanText.Text = "Belum Menikah";
            PekerjaanText.Text = "Developer";
            KewarganegaraanText.Text = "WNI";
        }

        private void OnBackButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(_appState);
            secondWindow.Show();
            this.Close();
        }

        private void OnRetryButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(_appState);
            secondWindow.Show();
            this.Close();
        }
    }
}
