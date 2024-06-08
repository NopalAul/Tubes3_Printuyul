using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace newjeans_avalonia
{
    public partial class ThirdWindow : Window
    {
        private AppState _appState;

        public ThirdWindow(AppState appState)
        {
            InitializeComponent();
            _appState = appState;

            // LoadingImage.IsVisible = false;
            FetchAndDisplayBiodata();

            this.FindControl<Button>("BackButton")!.Click += OnBackButtonClick;
            this.FindControl<Button>("RetryButton")!.Click += OnRetryButtonClick;
        }

        private async void FetchAndDisplayBiodata()
        {
            // try
            // {
            //     LoadingImage.IsVisible = true;
            if (_appState.ResultImage != null)
            {
                // string filename = _appState.ResultImageFilename;
                // Console.WriteLine(filename);
                // string apiUrl = $"http://localhost:5141/api/fingerprint/biodata/{filename}";

                // using (var client = new HttpClient())
                // {
                //     var response = await client.GetAsync(apiUrl);
                //     if (response.IsSuccessStatusCode)
                //     {
                //         var biodata = await response.Content.ReadAsAsync<KTPData>();

                NamaText.Text = _appState.ktpData.name;
                NikText.Text = _appState.ktpData.NIK;
                TempatLahirText.Text = _appState.ktpData.birth_place;
                TanggalLahirText.Text = _appState.ktpData.birth_date;
                JenisKelaminText.Text = _appState.ktpData.gender;
                GolonganDarahText.Text = _appState.ktpData.blood_type;
                AlamatText.Text = _appState.ktpData.address;
                AgamaText.Text = _appState.ktpData.religion;
                StatusPerkawinanText.Text = _appState.ktpData.marriage_status;
                PekerjaanText.Text = _appState.ktpData.job;
                KewarganegaraanText.Text = _appState.ktpData.citizenhip;

                // NamaText.Text = biodata.name;
                // NikText.Text = biodata.NIK;
                // TempatLahirText.Text = biodata.birth_place;
                // TanggalLahirText.Text = biodata.birth_date;
                // JenisKelaminText.Text = biodata.gender;
                // GolonganDarahText.Text = biodata.blood_type;
                // AlamatText.Text = biodata.address;
                // AgamaText.Text = biodata.religion;
                // StatusPerkawinanText.Text = biodata.marriage_status;
                // PekerjaanText.Text = biodata.job;
                // KewarganegaraanText.Text = biodata.citizenhip;
                //     }
                //     else
                //     {
                //         await ShowMessageAsync("No matching biodata found.");
                //     }
                // }
            }
            // catch (Exception ex)
            // {
            //     await ShowMessageAsync($"Exception occurred while fetching biodata: {ex.Message}");
            // } 
            // finally
            // {
            //     LoadingImage.IsVisible = false;
            // }
        }

        private async Task ShowMessageAsync(string message)
        {
            var messageBox = new MessageBox { Message = message };
            await messageBox.ShowDialog(this);
        }

        private void OnBackButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(_appState);
            secondWindow.Show();
            this.Close();
        }

        private void OnRetryButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ResetAppState();
            SecondWindow secondWindow = new SecondWindow(_appState);
            secondWindow.Show();
            this.Close();
        }

        private void ResetAppState()
        {
            _appState.CurrentImage = null;
            _appState.ResultImage = null;
            _appState.SelectedAlgorithm = null;
            _appState.Similarity = string.Empty;
            _appState.ExecutionTime = string.Empty;
            _appState.ResultImageFilename = null;
            _appState.ktpData = new KTPData("default_NIK", "default_name", "default_birth_place", "default_birth_date",
                                            "default_gender", "default_blood_type", "default_address", "default_religion",
                                            "default_marriage_status", "default_job", "default_citizenship");
        }
    }
}
