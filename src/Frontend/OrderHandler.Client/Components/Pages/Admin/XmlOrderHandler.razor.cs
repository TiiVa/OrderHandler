using System.Xml.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;
using OrderHandler.DataTransferContracts.DTOs.Product;
using OrderHandler.DataTransferContracts.DTOs.User;
using OrderHandler.DataTransferContracts.DTOs.XmlOrderDtos;

namespace OrderHandler.Client.Components.Pages.Admin;

public partial class XmlOrderHandler : ComponentBase
{

    [Inject] private IConfiguration config { get; set; }
    [Inject] private IProductService ProductService { get; set; }
    [Inject] private IOrderService OrderService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private XmlOrderDto xmlOrderDto { get; set; } = new();
    private List<OrderDto> OrderDtoList { get; set; } = [];
    private List<ProductDto> ProductDtoList { get; set; } = [];
    private List<UserDto> UserDtoList { get; set; } = [];
    private IBrowserFile? file;
    private long maxFileSize = 1024 * 1024 * 3;
    private int maxAllowedFiles = 3;
    private List<string> errors = new();

    protected override async Task OnInitializedAsync()
    {
        OrderDtoList.AddRange(await OrderService.GetAllAsync());
        ProductDtoList.AddRange(await ProductService.GetAllAsync());
        UserDtoList.AddRange(await UserService.GetAllAsync());
    }

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        file = e.File;
    }

    private async Task<string> CaptureFile()
    {
        if (file is null)
        {
            return "";
        }

        try
        {
            string newFileName = Path.ChangeExtension(
                Path.GetRandomFileName(),
                Path.GetExtension(file.Name));

            string path = Path.Combine(
                config.GetValue<string>("FileStorage")!,
                "bbrorsson",
                newFileName);

            string relativePath = Path.Combine("bbrorsson", newFileName);

            Directory.CreateDirectory(Path.Combine(config.GetValue<string>("FileStorage")!,
                "bbrorsson"));

            await using FileStream fs = new(path, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

            return path;
        }
        catch (Exception ex)
        {
            errors.Add($"File: {file.Name} Error: {ex.Message}");
            throw;
        }
    }

    private async Task SubmitFile()
    {
        try
        {
            string relativePath = await CaptureFile();

            DeserializeXMLFileToObject(relativePath);
        }
        catch (Exception ex)
        {
            errors.Add($"Error: {ex.Message}");
        }
    }

    private void DeserializeXMLFileToObject(string xmlPath)
    {
        var xmlSerializer = new XmlSerializer(typeof(XmlOrderDto));
        using (var reader = new StreamReader(xmlPath))
        {
            var order = (XmlOrderDto)xmlSerializer.Deserialize(reader);

            MappingFileToObjects(order);
        }
    }

    private async Task MappingFileToObjects(XmlOrderDto orderDto)
    {
        var userExists = UserDtoList.Any(u => u.Email == orderDto.XmlUser.Email);
        double totalForOrder = 0;

        if (!userExists)
        {
            var newUser = new UserDto
            {
                FirstName = orderDto.XmlUser.FirstName,
                LastName = orderDto.XmlUser.FirstName,
                Email = orderDto.XmlUser.Email,
                Phone = orderDto.XmlUser.Phone,
                Password = orderDto.XmlUser.Password,
                Role = "User",
                StreetAddress = orderDto.XmlUser.StreetAddress,
                ZipCode = "000 00",
                City = orderDto.XmlUser.City,
                Country = orderDto.XmlUser.Country,
                Orders = new List<OrderDto>(),
                IsActive = true
            };

            await UserService.AddAsync(newUser);
        }

        foreach (var product in orderDto.OrderProducts)
        {
            totalForOrder += product.XmlProduct.Price;

            var productExists = ProductDtoList.Any(op =>
                string.Equals(op.Name, product.XmlProduct.Name, StringComparison.CurrentCultureIgnoreCase));

            if (!productExists)
            {
                var newProduct = new ProductDto
                {
                    Name = product.XmlProduct.Name,
                    Description = product.XmlProduct.Description,
                    Price = product.XmlProduct.Price,
                    Color = product.XmlProduct.Color,
                    IsActive = true,
                    ProductOrders = new List<OrderProductDto>()
                };

                await ProductService.AddAsync(newProduct);

                ProductDtoList.Clear();
                ProductDtoList.AddRange(await ProductService.GetAllAsync());
            }
        }

        var userForOrder = UserDtoList.FirstOrDefault(u => u.Email == orderDto.XmlUser.Email);

        var newOrder = new OrderDto
        {
            OrderSum = totalForOrder,
            OrderProducts = orderDto.OrderProducts.Select(op => new OrderProductDto
            {
                OrderId = op.OrderId,
                Name = op.XmlProduct.Name,
                Description = op.XmlProduct.Description,
                Price = op.XmlProduct.Price,
                Quantity = op.Quantity,
                ProductId = ProductDtoList.FirstOrDefault(p => p.Name == op.XmlProduct.Name).Id
            }).ToList(),
            UserId = userForOrder.Id,
            IsActive = true
        };

        await OrderService.AddAsync(newOrder);
    }

    private void BackToOverView()
    {
        NavigationManager.NavigateTo("/orders");
    }
}