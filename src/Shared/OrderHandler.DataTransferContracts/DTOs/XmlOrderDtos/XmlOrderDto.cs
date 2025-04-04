using System.Xml.Serialization;

namespace OrderHandler.DataTransferContracts.DTOs.XmlOrderDtos;
[XmlRoot("XmlOrderDto")]
[Serializable]
public class XmlOrderDto
{
    public int Id { get; set; }
    [XmlElement("TotalSum")]
    public double OrderSum { get; set; }
    [XmlArray("ArticlesInOrderList")]
    [XmlArrayItem("XmlArticlesInOrderDto")]
    public List<XmlOrderProductDto> OrderProducts { get; set; } = new();
    [XmlElement("Company")]
    public XmlUserDto XmlUser { get; set; }
    public bool IsActive { get; set; }
}