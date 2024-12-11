namespace GlobalCoders.PSP.BackendApi.Identity.Models;

public sealed class ProfileUserInfo
{
    public required string Email { get; set; }
    public required IReadOnlyList<string> Roles { get; set; }
    public required bool IsEmailConfirmed { get; set; }
}
