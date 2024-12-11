using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Controllers;

public class DiscountController : BaseApiController
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet("[action]/{id:guid}")]
    public async Task<ActionResult<DiscountResponseModel>> Id(Guid id)
    {
        var discount = await _discountService.GetAsync(id);
        if (discount == null) return NotFound();
        return Ok(discount);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<DiscountListModel>>> All(DiscountFilter filter)
    {
        var result = await _discountService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(DiscountCreateModel createModel)
    {
        var success = await _discountService.CreateAsync(createModel);
        return success ? Ok() : Problem("Failed to create discount");
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> Update(DiscountUpdateModel updateModel)
    {
        var success = await _discountService.UpdateAsync(updateModel);
        return success ? Ok() : Problem("Failed to update discount");
    }

    [HttpDelete("[action]/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _discountService.DeleteAsync(id);
        return success ? Ok() : Problem("Failed to delete discount");
    }
}