namespace GlobalCoders.PSP.BackendApi.Base.Models;

public sealed class ValidationDetails
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
