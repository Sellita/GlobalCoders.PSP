namespace GlobalCoders.PSP.BackendApi.Email.Configuration;

public sealed class MailConfiguration
{
    public const string SectionName = "Mail";

    public required string From { get; set; }
    public required string FromName { get; set; }

    public required string ReplayTo { get; set; }
    public required string ReplayToName { get; set; }
}
