using System.Xml.Serialization;
using OrderHandler.DataTransferContracts.DTOs.XmlOrderDtos;

namespace OrderHandler.DataTransferContracts.DTOs.XmlOrderDtos;

[Serializable]
public class XmlUserDto
{
    public int Id { get; set; }
    [XmlElement("CompanyName")]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [XmlElement("Email")]
    public string Email { get; set; }
    [XmlElement("Phone")]
    public string Phone { get; set; }
    [XmlElement("Password")]
    public string Password { get; set; }
    public string Role { get; set; }
    [XmlElement("Address")]
    public string StreetAddress { get; set; }
    public string? ZipCode { get; set; }
    [XmlElement("City")]
    public string City { get; set; }
    [XmlElement("Country")]
    public string Country { get; set; }
    public List<XmlOrderDto> Orders { get; set; } = new();
    public bool? IsActive { get; set; }
}