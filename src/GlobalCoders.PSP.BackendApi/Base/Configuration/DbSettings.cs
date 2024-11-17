namespace GlobalCoders.PSP.BackendApi.Base.Configuration;

public record DbSettings
{
    public const string SectionName = "DbSettings";
    public string ConnectionString { get; set; } = string.Empty;

    public int ReconnectionTimeoutInMs { get; set; }
    public int WaitForRetrySeconds { get; set; }

    public int MaxConnections { get; set; }
}
