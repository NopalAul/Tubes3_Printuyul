using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Markup.Xaml;

namespace newjeans_avalonia
{
    public partial class ThirdWindow : Window
    {
        private Bitmap? currentImage;

        public ThirdWindow(Bitmap? image = null)
        {
            InitializeComponent();
            currentImage = image;

            // Bind dummy data to text placeholders
            BindDummyData();
            
            this.FindControl<Button>("BackButton")!.Click += OnBackButtonClick;
            this.FindControl<Button>("RetryButton")!.Click += OnRetryButtonClick;
        }

        private void BindDummyData()
        {
            // Create and bind dummy data to text placeholders
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
            SecondWindow secondWindow = new SecondWindow(currentImage);
            secondWindow.Show();
            this.Close();
        }

        private void OnRetryButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow();
            secondWindow.Show();
            this.Close();
        }
    }
}
