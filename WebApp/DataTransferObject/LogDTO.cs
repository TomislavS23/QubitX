namespace WebApp.DataTransferObject;

public class LogDTO
{
    public DateTime? LogTimestamp { get; set; }

    public byte? LogLevel { get; set; }

    public string? LogMessage { get; set; }
}