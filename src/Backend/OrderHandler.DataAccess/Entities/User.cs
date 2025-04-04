using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.EntityInterfaces;

namespace OrderHandler.DataAccess.Entities
{
    public class User : IUser, IEntity<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? StreetAddress { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public bool IsActive { get; set; }
    }
}
