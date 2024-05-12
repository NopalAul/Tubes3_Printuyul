using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Collections.Generic; // untuk bisa pakai list


namespace newjeans_avalonia;

public partial class ThirdWindow : Window
{
    private Bitmap? currentImage;

    public ThirdWindow(Bitmap? image = null)
    {
        InitializeComponent();
        currentImage = image;

        this.FindControl<Button>("BackButton")!.Click += OnBackButtonClick;
        this.FindControl<Button>("RetryButton")!.Click += OnRetryButtonClick;
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
