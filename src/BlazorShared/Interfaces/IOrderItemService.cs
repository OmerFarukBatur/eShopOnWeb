using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models;

namespace BlazorShared.Interfaces;
public interface IOrderItemService
{
    Task<List<AllOrders>> List();
    Task<OrderByIdItemsResponse> GetById(int id);
    Task OrderStatusUpdate(int id );
}
