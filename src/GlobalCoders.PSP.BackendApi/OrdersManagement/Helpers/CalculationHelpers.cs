using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Helpers;

public static class CalculationHelpers
{
    public static decimal CalculateTotalPrice(OrderEntity orderEntity)
    {
        return orderEntity.OrderProducts.Sum(x => x.Price * x.Quantity) +
            orderEntity.OrderProducts.Sum(x => x.OrderProductTaxes.Sum(y=>y.Value) * x.Quantity) - orderEntity.Discount + orderEntity.Tips;
    }
    
    public static decimal CalculatePriceWithTax(OrderEntity orderEntity)
    {
        return orderEntity.OrderProducts.Sum(x => x.Price * x.Quantity) +
               orderEntity.OrderProducts.Sum(x => x.OrderProductTaxes.Sum(y => y.Value) * x.Quantity);
    }

    public static decimal CalculateLeftToPay(OrderEntity order)
    {
        var totalPaid = GetTotalPaid(order);
        return CalculateTotalPrice(order) - totalPaid;
    }

    private static decimal GetTotalPaid(OrderEntity order)
    {
        return order.OrderPayments.Sum(x => x.Amount);
    }
    
    public static decimal RoundToTwoDecimalPlaces(decimal value)
    {
        return Math.Round(value, 2);
    }
}