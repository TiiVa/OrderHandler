using System.Xml.Serialization;

namespace OrderHandler.DataTransferContracts.DTOs.XmlOrderDtos;
[Serializable]
public class XmlProductDto
{
    public string Id { get; set; }
    [XmlElement("Name")]
    public string Name { get; set; }
    [XmlElement("Description")]
    public string Description { get; set; }
    [XmlElement("Price")]
    public double Price { get; set; }
    public string? Color { get; set; }
    [XmlElement("Status")]
    public bool IsActive { get; set; }
}