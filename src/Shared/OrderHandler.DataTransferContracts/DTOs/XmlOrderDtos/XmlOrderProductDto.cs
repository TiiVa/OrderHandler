using System.Xml.Serialization;


namespace OrderHandler.DataTransferContracts.DTOs.XmlOrderDtos;
[Serializable]
public class XmlOrderProductDto
{

    public int OrderId { get; set; }
    [XmlElement("Article")]
    public XmlProductDto XmlProduct { get; set; }
    public int ProductId { get; set; }

    [XmlElement("Amount")]
    public int Quantity { get; set; }
}