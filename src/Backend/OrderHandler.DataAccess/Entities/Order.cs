using System.ComponentModel.DataAnnotations;
using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.EntityInterfaces;

namespace OrderHandler.DataAccess.Entities
{
    public class Order : IOrder, IEntity<int>
    {
        public int Id { get; set; }
        public double OrderSum { get; set; }
        [Required]
        public User User { get; set; }
        public bool IsActive { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
