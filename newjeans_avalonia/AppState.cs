using Avalonia.Media.Imaging;

public class AppState
{
    public Bitmap? CurrentImage { get; set; }
    public Bitmap? ResultImage { get; set; }
    public string? SelectedAlgorithm { get; set; }
    public string Similarity { get; set; }
    public string ExecutionTime { get; set; }
    public string? ResultImageFilename { get; set; }
}
