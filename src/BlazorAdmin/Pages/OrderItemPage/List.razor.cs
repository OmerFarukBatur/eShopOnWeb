using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorAdmin.Helpers;
using BlazorShared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Pages.OrderItemPage;

public partial class List : BlazorComponent
{
    //[Inject]
    //public IOrderItemService OrderItemService { get; set; }

    [Inject]
    public HttpClient Http {  get; set; }

    

    private List<AllOrders> allOrders = new List<AllOrders>();
    private Details DetailsComponent { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    { 

        if (firstRender)
        {
            allOrders = await Http.GetFromJsonAsync<List<AllOrders>>("/Service");
            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async void DetailsClick(int id)
    {
        await DetailsComponent.Open(id);
    }

    private async Task ReloadCatalogItems()
    {
        allOrders = await Http.GetFromJsonAsync<List<AllOrders>>("https://localhost:44315/Deneme");
        StateHasChanged();
    }
}
