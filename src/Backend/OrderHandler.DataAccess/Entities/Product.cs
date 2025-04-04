using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.EntityInterfaces;

namespace OrderHandler.DataAccess.Entities
{
    public class Product : IProduct, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<OrderProduct> ProductOrders { get; set; }

    }
}
