using Avalonia.Media.Imaging;

public class AppState
{
    public Bitmap? CurrentImage { get; set; }
    public string? SelectedAlgorithm { get; set; }
    public string Similarity { get; set; } = string.Empty;
    public string ExecutionTime { get; set; } = string.Empty;
    public Bitmap? ResultImage { get; set; } 
}