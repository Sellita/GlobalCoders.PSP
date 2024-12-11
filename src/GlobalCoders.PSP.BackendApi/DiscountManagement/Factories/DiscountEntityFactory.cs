using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;

public static class DiscountEntityFactory
{
    public static DiscountEntity Create(DiscountCreateModel model)
    {
        return new DiscountEntity
        {
            Code = model.Code,
            Description = model.Description,
            Percentage = model.Percentage,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            UsageLimit = model.UsageLimit
        };
    }

    public static DiscountEntity Update(DiscountUpdateModel model)
    {
        return new DiscountEntity
        {
            Id = model.Id,
            Code = model.Code,
            Description = model.Description,
            Percentage = model.Percentage,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            UsageLimit = model.UsageLimit
        };
    }
}