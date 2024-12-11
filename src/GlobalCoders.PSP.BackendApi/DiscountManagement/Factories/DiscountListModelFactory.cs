using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;

public static class DiscountListModelFactory
{
    public static DiscountListModel Create(DiscountEntity entity)
    {
        return new DiscountListModel
        {
            Id = entity.Id,
            Code = entity.Code,
            Description = entity.Description,
            Percentage = entity.Percentage,
            UsageLimit = entity.UsageLimit,
            UsageCount = entity.UsageCount,
            Status = entity.Status,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };
    }
}
