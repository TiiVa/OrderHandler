using OrderHandler.DataTransferContracts.DTOs.OrderProduct;

namespace OrderHandler.Client.Services;

public class CartService
{
    
    public List<OrderProductDto> CartProducts { get; set; } = new();
    public int CartBadgeQuantity => CartProducts.Count;



    public void AddProductToCart(OrderProductDto product)
    {
        CartProducts.Add(product);
    }

    public void RemoveProductFromCart(OrderProductDto product)
    {
        CartProducts.Remove(product);
    }


    public double TotalForCart()
    {
        double total = 0;

        foreach (var cartProduct in CartProducts)
        {
            total += cartProduct.Price;
        }

        return total;
    }

    public int CountQuantity(OrderProductDto productDto)
    {
        var totalQuantity = CartProducts.Count(p => p.ProductId == productDto.ProductId);

        return totalQuantity;
    }

    
}