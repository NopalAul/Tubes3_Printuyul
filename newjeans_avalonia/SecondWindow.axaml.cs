using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Collections.Generic; // untuk bisa pakai list


namespace newjeans_avalonia;

public partial class SecondWindow : Window
{
    public SecondWindow(Bitmap? initialImage = null)
    {
        InitializeComponent();
        InsertButton.Click += InsertButton_Click;
        this.FindControl<Button>("NavigateButton2")!.Click += OnNavigateButtonClick2;

        if (initialImage != null)
        {
            DisplayImage.Source = initialImage;
        }
    }

    private async void InsertButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select an Image",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Image Files", Extensions = new List<string> { "jpg", "jpeg", "png", "bmp" } }
            },
            AllowMultiple = false
        };

        var result = await dialog.ShowAsync(this);
        if (result != null && result.Any())
        {
            var bitmap = new Bitmap(result.First());
            DisplayImage.Source = bitmap;
        }
    }

    private void OnNavigateButtonClick2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Bitmap? currentBitmap = DisplayImage.Source as Bitmap;
        ThirdWindow thirdWindow = new ThirdWindow(currentBitmap);
        thirdWindow.Show();
        this.Close(); // Optionally close the current window
    }
}
