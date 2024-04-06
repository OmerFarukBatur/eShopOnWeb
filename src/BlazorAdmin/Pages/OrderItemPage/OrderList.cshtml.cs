using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorAdmin.Pages.OrderItemPage
{
    public class OrderListModel : PageModel
    {
        private readonly IOrderItemService _orderItemService;

        public OrderListModel(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        public List<AllOrders> allOrders;

        public async Task OnGet()
        {
            allOrders= await _orderItemService.List();
        }
    }
}
