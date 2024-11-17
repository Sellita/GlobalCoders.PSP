namespace GlobalCoders.PSP.BackendApi.Identity.Models;

public sealed class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
