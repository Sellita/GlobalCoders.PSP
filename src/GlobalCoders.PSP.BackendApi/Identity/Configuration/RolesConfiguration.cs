using GlobalCoders.PSP.BackendApi.Base.Class;

namespace GlobalCoders.PSP.BackendApi.Identity.Configuration;

public sealed class RolesConfiguration : CaseInSensitiveDictionary<HashSet<string>>
{
    public const string SectionName = "Roles";
}
