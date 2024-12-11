using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Extensions
{
    public static class DiscountModelExtension
    {
        /// <summary>
        /// Converts a DiscountCreateModel to a DiscountEntity.
        /// </summary>
        public static DiscountEntity ToEntity(this DiscountCreateModel model)
        {
            return new DiscountEntity
            {
                Code = model.Code,
                Description = model.Description,
                Percentage = model.Percentage,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UsageLimit = model.UsageLimit,
                CreatedDateTime = DateTime.UtcNow,
                Status = "Active"
            };
        }

        /// <summary>
        /// Updates a DiscountEntity with values from a DiscountUpdateModel.
        /// </summary>
        public static DiscountEntity UpdateEntity(this DiscountEntity entity, DiscountUpdateModel model)
        {
            entity.Code = model.Code;
            entity.Description = model.Description;
            entity.Percentage = model.Percentage;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.UsageLimit = model.UsageLimit;
            entity.Status = model.Status;
            return entity;
        }
    }
}