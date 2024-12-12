namespace GlobalCoders.PSP.BackendApi.Base.Configuration;

public class CorsConfiguration
{
    public const string SectionName = "Cors";

    public List<string> Origins { get; set; } = new();
}
