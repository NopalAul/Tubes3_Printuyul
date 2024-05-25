using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System;

namespace newjeans_avalonia
{
    public partial class SecondWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public SecondWindow(Bitmap? initialImage = null)
        {
            InitializeComponent();
            InsertButton.Click += InsertButton_Click;
            SearchButton.Click += OnSearchButtonClick;
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

        private async void OnSearchButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var selectedAlgorithm = (MethodDropdown.SelectedItem as ComboBoxItem)?.Content.ToString();
            Console.WriteLine($"dropdown: {selectedAlgorithm}");
            if (selectedAlgorithm == "Boyer Moore")
            {
                selectedAlgorithm = "BM";
            }
            else if (selectedAlgorithm == "Knuth Morris Pratt")
            {
                selectedAlgorithm = "KMP";
            }
            Console.WriteLine($"{selectedAlgorithm}");

            if (DisplayImage.Source is Bitmap currentBitmap && !string.IsNullOrEmpty(selectedAlgorithm))
            {
                LoadingImage.IsVisible = true;
                await ProcessImageAsync(currentBitmap, selectedAlgorithm);
                LoadingImage.IsVisible = false;
            }
            else
            {
                await ShowMessageAsync("Please select an image and an algorithm.");
            }
        }

        private async Task ProcessImageAsync(Bitmap image, string algorithm)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5141/");
                    var content = new MultipartFormDataContent();

                    var memoryStream = new MemoryStream();
                    image.Save(memoryStream);
                    memoryStream.Position = 0;

                    var imageContent = new StreamContent(memoryStream);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    content.Add(imageContent, "image", "uploadedImage.png");
                    content.Add(new StringContent(algorithm), "algorithm");

                    var response = await client.PostAsync("api/fingerprint/process", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<dynamic>();
                        string similarImage = result.similarImage ?? "N/A";
                        double percentage = result.percentage ?? 0;
                        long executionTime = result.executionTime ?? 0; // Execution time in milliseconds
                        bool exactMatchFound = result.exactMatchFound ?? false; // Handle null case

                        string imageUrl = $"http://localhost:5141/api/fingerprint/image/{similarImage}";
                        await FetchAndDisplayImageAsync(imageUrl);

                        SimilarityTextBlock.Text = $"{percentage} %";
                        ExecutionTimeTextBlock.Text = $"{executionTime} ms";

                        if (!exactMatchFound)
                        {
                            await ShowMessageAsync("No exact match found. The search will continue using Hamming Distance.");
                        }
                    }
                    else
                    {
                        await ShowMessageAsync("Error processing image.");
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"Exception occurred: {ex.Message}");
            }
        }

        private async Task ShowMessageAsync(string message)
        {
            var messageBox = new MessageBox { Message = message };
            await messageBox.ShowDialog(this);
        }


        private async Task FetchAndDisplayImageAsync(string imageUrl)
        {
            try
            {
                var response = await client.GetAsync(imageUrl).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                    // image load and setting source
                    await Task.Run(async () =>
                    {
                        using (var stream = new MemoryStream(imageBytes))
                        {
                            var bitmap = new Bitmap(stream);

                            // main thread update ui
                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                ResultsImage.Source = bitmap;
                                LoadingImage.IsVisible = false; // Hide loading image
                            });
                        }
                    });
                }
                else
                {
                    await ShowMessageAsync("Error fetching image from the server.");
                    LoadingImage.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"Exception occurred while fetching the image: {ex.Message}");
                LoadingImage.IsVisible = false;
            }
        }

        // kalau sempet nanti message box nya bikin manual (jangan pakai default jele hehe)
        // private async Task ShowMessageAsync(string message)
        // {
        //     var dialog = new Window
        //     {
        //         Title = "Message",
        //         Content = new TextBlock { Text = message },
        //         Width = 400,
        //         Height = 200
        //     };
        //     await dialog.ShowDialog(this);
        // }
    }
}
