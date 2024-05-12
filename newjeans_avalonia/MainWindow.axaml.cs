// MainWindow.axaml.cs
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace newjeans_avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.FindControl<Button>("NavigateButton")!.Click += OnNavigateButtonClick;
    }

    private void OnNavigateButtonClick(object? sender, RoutedEventArgs e)
    {
        SecondWindow secondWindow = new SecondWindow();
        secondWindow.Show();
        this.Close(); // Optionally close the current window
    }
}
