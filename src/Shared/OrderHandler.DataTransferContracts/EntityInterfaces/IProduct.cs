using System.ComponentModel;

namespace OrderHandler.DataTransferContracts.EntityInterfaces
{
    public interface IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; }

    }
}
