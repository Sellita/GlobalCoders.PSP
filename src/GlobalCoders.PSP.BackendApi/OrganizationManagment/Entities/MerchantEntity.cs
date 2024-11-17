using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;

public class MerchantEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string DisplayName { get; set; } = string.Empty;
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string LegalName { get; set; }= string.Empty;
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Address { get; set; }= string.Empty;
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Email { get; set; }= string.Empty;
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string MainPhoneNr { get; set; }= string.Empty;
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string SecondaryPhoneNr { get; set; }= string.Empty;
    public TimeSpan OpeningHour { get; set; }
    public TimeSpan ClosingHour { get; set; }
    public TimeSpan BatchOutTime { get; set; }
    
    public bool IsDeleted { get; set; }
}