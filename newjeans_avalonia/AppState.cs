using Avalonia.Media.Imaging;

namespace newjeans_avalonia
{
    public class AppState
    {
        public Bitmap? CurrentImage { get; set; }
        public Bitmap? ResultImage { get; set; }
        public string? SelectedAlgorithm { get; set; }
        public string Similarity { get; set; }
        public string ExecutionTime { get; set; }
        public string? ResultImageFilename { get; set; }
        public KTPData ktpData { get; set; }

        public AppState()
        {
            // Example values, replace them with appropriate values
            ktpData = new KTPData("default_NIK", "default_name", "default_birth_place", "default_birth_date",
                                  "default_gender", "default_blood_type", "default_address", "default_religion",
                                  "default_marriage_status", "default_job", "default_citizenship");
        }

    }
}
