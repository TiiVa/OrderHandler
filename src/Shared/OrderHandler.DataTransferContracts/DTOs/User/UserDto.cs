using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.DataTransferContracts.DTOs.User;

public class UserDto : IEntity<int>
{
    public int Id { get; set; }
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    [EmailAddress(ErrorMessage = "Only email format")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required, no empty strings")]
    public string Email { get; set; }
    [Phone]
    [Required(ErrorMessage = "Phone number is required")]
    public string Phone { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public string Role { get; set; }
    public string? StreetAddress { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    [JsonIgnore]
    public List<OrderDto> Orders { get; set; } = new();
    public bool IsActive { get; set; }
}