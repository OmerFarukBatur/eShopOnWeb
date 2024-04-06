using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Logging;


namespace BlazorAdmin.Services;

public class OrderItemService : IOrderItemService
{
    private readonly CatalogContext _catalogContext;
    private readonly AppIdentityDbContext _appIdentityDbContext;
    private readonly HttpService _httpService;
    private readonly ILogger<OrderItemService> _logger;

    public OrderItemService(
        CatalogContext catalogContext, 
        AppIdentityDbContext appIdentityDbContext, 
        HttpService httpService, 
        ILogger<OrderItemService> logger
        )
    {
        _catalogContext = catalogContext;
        _appIdentityDbContext = appIdentityDbContext;
        _httpService = httpService;
        _logger = logger;
    }


    //public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    //{
    //    var response = await _httpService.HttpPost<CreateCatalogItemResponse>("catalog-items", catalogItem);
    //    return response?.CatalogItem;
    //}

    //public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    //{
    //    return (await _httpService.HttpPut<EditCatalogItemResult>("catalog-items", catalogItem)).CatalogItem;
    //}

    //public async Task<string> Delete(int catalogItemId)
    //{
    //    return (await _httpService.HttpDelete<DeleteCatalogItemResponse>("catalog-items", catalogItemId)).Status;
    //}

    //public async Task<CatalogItem> GetById(int id)
    //{
    //    var brandListTask = _brandService.List();
    //    var typeListTask = _typeService.List();
    //    var itemGetTask = _httpService.HttpGet<EditCatalogItemResult>($"catalog-items/{id}");
    //    await Task.WhenAll(brandListTask, typeListTask, itemGetTask);
    //    var brands = brandListTask.Result;
    //    var types = typeListTask.Result;
    //    var catalogItem = itemGetTask.Result.CatalogItem;
    //    catalogItem.CatalogBrand = brands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId)?.Name;
    //    catalogItem.CatalogType = types.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId)?.Name;
    //    return catalogItem;
    //}

    //public async Task<List<CatalogItem>> ListPaged(int pageSize)
    //{
    //    _logger.LogInformation("Fetching catalog items from API.");

    //    var brandListTask = _brandService.List();
    //    var typeListTask = _typeService.List();
    //    var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items?PageSize=10");
    //    await Task.WhenAll(brandListTask, typeListTask, itemListTask);
    //    var brands = brandListTask.Result;
    //    var types = typeListTask.Result;
    //    var items = itemListTask.Result.CatalogItems;
    //    foreach (var item in items)
    //    {
    //        item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
    //        item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
    //    }
    //    return items;
    //}

    public async Task<List<AllOrders>> List()
    {
        _logger.LogInformation("Fetching catalog items from API.");

        List<ApplicationUser> user = await _appIdentityDbContext.Users.ToListAsync();

        List<AllOrders> orders = await _catalogContext.Orders.Include(o=> o.OrderItems).Select(z => new AllOrders
        {
            BuyerId = z.BuyerId,
            OrderDate = z.OrderDate,
            OrderId = z.Id,
            Status = z.Status,
            Total = z.OrderItems.Sum(y => y.UnitPrice * y.Units)
        }).ToListAsync();

        orders = orders.Join(user,x=> x.BuyerId,y=> y.Email, (x, y) => new{ x,y}).Select(a=> new AllOrders
        {
            UserName = a.y.UserName,
            Email = a.y.Email,
            BuyerId = a.x.BuyerId,
            OrderDate = a.x.OrderDate,
            OrderId = a.x.OrderId,
            Status = a.x.Status,
            Total = a.x.Total
        }).ToList();
        return orders;
    }

    public async Task<OrderByIdItemsResponse> GetById(int id)
    {
        bool status = await _catalogContext.Orders.Where(x => x.Id == id).Select(y => y.Status).FirstAsync();
        List<OrderByIdItems> items = await _catalogContext.OrderItems.Where(x=> x.OrderId == id).Select(y=> new OrderByIdItems
        {
            OrderId = y.OrderId,
            Id = y.Id,
            CatalogId = y.ItemOrdered.CatalogItemId,
            PictureUri = y.ItemOrdered.PictureUri,
            ProductName = y.ItemOrdered.ProductName,
            Unit = y.Units,
            UnitPrice = y.UnitPrice
        }).ToListAsync();


        return new() { 
            OrderItems = items,
            OrderStatus = status
        };
    }

    public async Task OrderStatusUpdate(int id )
    {
        Order order = await _catalogContext.Orders.FirstAsync(x=> x.Id == id);

        order.Status = !order.Status;

        _catalogContext.Orders.Update(order);

        await _catalogContext.SaveChangesAsync();
    }
}
