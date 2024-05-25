// MainWindow.axaml.cs
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace newjeans_avalonia
{
    public partial class MainWindow : Window
    {
        private AppState _appState;

        public MainWindow()
        {
            InitializeComponent();
            _appState = new AppState(); // Initialize the AppState
            this.FindControl<Button>("NavigateButton")!.Click += OnNavigateButtonClick;
        }

        private void OnNavigateButtonClick(object? sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(_appState);
            secondWindow.Show();
            this.Close(); // Optionally close the current window
        }
    }
}
