using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAdmin.Pages.OrderItemPage;


public class ServiceController : Controller
{
    private readonly IOrderItemService _orderItemService;

    public ServiceController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<AllOrders> orders = await _orderItemService.List();
        return Ok(orders);
    }

    public async Task<IActionResult> Detail(int id)
    {
        OrderByIdItemsResponse orderByIdItems = await _orderItemService.GetById(id);
        return Ok(orderByIdItems);
    }


    [HttpPost("{id}/{status}")]
    public async Task<IActionResult>  UpdateOrderStatus(int id)
    {
        await _orderItemService.OrderStatusUpdate(id);
        return Ok();
    }

}
