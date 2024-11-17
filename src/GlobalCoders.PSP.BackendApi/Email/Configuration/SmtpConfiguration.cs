namespace GlobalCoders.PSP.BackendApi.Email.Configuration;

public sealed class SmtpConfiguration
{
    public const string SectionName = "SmtpProvider";

    public string? SmtpServer { get; set; }
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool EnableSsl { get; set; }
    public int TimeoutInSeconds { get; set; }
}
