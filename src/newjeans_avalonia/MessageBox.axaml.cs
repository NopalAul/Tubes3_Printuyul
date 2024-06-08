using Avalonia.Controls;
using Avalonia.Interactivity;

namespace newjeans_avalonia
{
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
        }

        public string Message
        {
            get => MessageTextBlock.Text!;
            set => MessageTextBlock.Text = value;
        }

        private void OkButton_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
