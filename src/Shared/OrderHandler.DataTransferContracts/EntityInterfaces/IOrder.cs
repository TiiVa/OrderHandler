using System.ComponentModel.DataAnnotations;

namespace OrderHandler.DataTransferContracts.EntityInterfaces
{
    public interface IOrder
    {
        public int Id { get; set; }
        public double OrderSum { get; set; }
        public bool IsActive { get; set; }
    }
}
