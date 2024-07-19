using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Log
{
    public int IdLog { get; set; }

    public DateTime? LogTimestamp { get; set; }

    public byte? LogLevel { get; set; }

    public string? LogMessage { get; set; }
}
