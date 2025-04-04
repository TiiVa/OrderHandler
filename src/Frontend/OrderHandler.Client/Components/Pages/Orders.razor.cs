using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Client.Components.Pages;

public partial class Orders : ComponentBase
{
    [Inject] private IOrderService OrderService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private List<OrderDto> OrderDtoList { get; set; } = new();

    [Parameter] public EventCallback OnAdd { get; set; }

    private PaginationState paginationState = new PaginationState { ItemsPerPage = 5 };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            OrderDtoList.AddRange(await OrderService.GetAllAsync());
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
   

    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    private void GoToOrderCancellation()
    {
        NavigationManager.NavigateTo("/admin/order_management/cancellation");
    }

    private void GoToExportOrders()
    {
        NavigationManager.NavigateTo("/admin/order_management/export");
    }

    private void GoToOrderImport()
    {
        NavigationManager.NavigateTo("/XmlOrderHandler");
    }
}