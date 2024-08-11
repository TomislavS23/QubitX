namespace WebApp.Models;

public class LogViewModel
{
    public DateTime? LogTimestamp { get; set; }

    public byte? LogLevel { get; set; }

    public string? LogMessage { get; set; }
}