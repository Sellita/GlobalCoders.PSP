using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Services;
using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.Tests.OrdersManagment.Services;

public class OrdersServiceTests
{
    [Fact]
    public void CalculateTaxTest()
    {
        //Arrange
        List<TaxListModel> taxes = new List<TaxListModel>()
        {
            new TaxListModel
            {
                Id = Guid.NewGuid(),
                DisplayName = "IVA",
                Type = TaxType.Percentage,
                Value = 21
            }
        };
        OrderProductEntity product = new OrderProductEntity
        {
            Price = 11,
            Quantity = 2
        };

        //Act
        var result = OrdersService.CalculateTaxes(taxes, product);
        //Assert
        Assert.Equal((decimal)2.31, result.First().Value);
    }
}