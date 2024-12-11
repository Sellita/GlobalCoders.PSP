using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;

public static class DiscountResponseModelFactory
{
    public static DiscountResponseModel Create(DiscountEntity entity)
    {
        return new DiscountResponseModel
        {
            Id = entity.Id,
            Code = entity.Code,
            Description = entity.Description,
            Percentage = entity.Percentage,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status,
            UsageLimit = entity.UsageLimit,
            UsageCount = entity.UsageCount
        };
    }
}