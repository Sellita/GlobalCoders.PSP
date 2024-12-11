using GlobalCoders.PSP.BackendApi.Base.Models;

namespace GlobalCoders.PSP.BackendApi.Base.Factories;

public static class ValidationDetailsFactory
{
    public static ValidationDetails Ok()
    {
        return new ValidationDetails
        {
            Success = true,
            ErrorMessage = string.Empty
        };
    }
    
    public static ValidationDetails Fail(string error)
    {
        return new ValidationDetails
        {
            Success = false,
            ErrorMessage = error
        };
    }
}
