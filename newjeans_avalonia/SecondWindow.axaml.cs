using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Collections.Generic; // untuk bisa pakai list


namespace newjeans_avalonia;

public partial class SecondWindow : Window
{
    public SecondWindow()
    {
        InitializeComponent();
        InsertButton.Click += InsertButton_Click;
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
}
