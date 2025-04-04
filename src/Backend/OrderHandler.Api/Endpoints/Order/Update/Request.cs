namespace OrderHandler.Api.Endpoints.Order.Update;

public class Request
{
    public int Id { get; set; }
    public double OrderSum { get; set; }
    public bool IsActive { get; set; }
}