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
        private AppState _appState;
        private static readonly HttpClient client = new HttpClient();

        public SecondWindow(AppState appState)
        {
            InitializeComponent();
            _appState = appState;
            InsertButton.Click += InsertButton_Click;
            SearchButton.Click += OnSearchButtonClick;
            this.FindControl<Button>("NavigateButton2")!.Click += OnNavigateButtonClick2;

            if (_appState.CurrentImage != null)
            {
                DisplayImage.Source = _appState.CurrentImage;
            }

            if (!string.IsNullOrEmpty(_appState.SelectedAlgorithm))
            {
                MethodDropdown.SelectedItem = MethodDropdown.Items.OfType<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content!.ToString() == _appState.SelectedAlgorithm);
            }

            SimilarityTextBlock.Text = _appState.Similarity;
            ExecutionTimeTextBlock.Text = _appState.ExecutionTime;

            if (_appState.ResultImage != null)
            {
                ResultsImage.Source = _appState.ResultImage;
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
                _appState.CurrentImage = bitmap; // Save the image in the state
            }
        }

        private async void OnNavigateButtonClick2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_appState.ResultImage == null)
            {
                await ShowMessageAsync("Please perform a search before seeing details.");
                return;
            }

            ThirdWindow thirdWindow = new ThirdWindow(_appState);
            thirdWindow.Show();
            this.Close(); // Optionally close the current window
        }

        private async void OnSearchButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var selectedAlgorithm = (MethodDropdown.SelectedItem as ComboBoxItem)?.Content.ToString();
            _appState.SelectedAlgorithm = selectedAlgorithm;

            if (selectedAlgorithm == "Boyer Moore")
            {
                selectedAlgorithm = "BM";
            }
            else if (selectedAlgorithm == "Knuth Morris Pratt")
            {
                selectedAlgorithm = "KMP";
            }

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
                    Console.WriteLine(imageUrl);
                    await FetchAndDisplayImageAsync(imageUrl);

                    _appState.ResultImageFilename = similarImage;
                    _appState.Similarity = $"{percentage} %";
                    _appState.ExecutionTime = $"{executionTime} ms";
                    SimilarityTextBlock.Text = _appState.Similarity;
                    ExecutionTimeTextBlock.Text = _appState.ExecutionTime;

                    string apiUrl = $"http://localhost:5141/api/fingerprint/biodata/{similarImage}";

                    var biodataResponse = await client.GetAsync(apiUrl);
                    if (biodataResponse.IsSuccessStatusCode)
                    {
                        var biodata = await biodataResponse.Content.ReadAsAsync<KTPData>();

                        _appState.ktpData.name = biodata.name;
                        _appState.ktpData.NIK = biodata.NIK;
                        _appState.ktpData.birth_place = biodata.birth_place;
                        _appState.ktpData.birth_date = biodata.birth_date;
                        _appState.ktpData.gender = biodata.gender;
                        _appState.ktpData.blood_type = biodata.blood_type;
                        _appState.ktpData.address = biodata.address;
                        _appState.ktpData.religion = biodata.religion;
                        _appState.ktpData.marriage_status = biodata.marriage_status;
                        _appState.ktpData.job = biodata.job;
                        _appState.ktpData.citizenhip = biodata.citizenhip;
                    }
                    else
                    {
                        await ShowMessageAsync("No matching biodata found.");
                    }

                    if (!exactMatchFound)
                    {
                        if (algorithm == "BM")
                        {
                            await ShowMessageAsync("No exact match found using BM. The search is done using Hamming Distance.");
                        }
                        else if (algorithm == "KMP")
                        {
                            await ShowMessageAsync("No exact match found using KMP. The search is done using Hamming Distance.");
                        }
                    }
                }
                else
                {
                    await ShowMessageAsync("Error processing image.");
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
                                _appState.ResultImage = bitmap;
                                LoadingImage.IsVisible = false;
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
    }
}
