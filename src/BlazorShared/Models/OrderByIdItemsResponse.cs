using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShared.Models;
public class OrderByIdItemsResponse
{
    public bool OrderStatus { get; set; }
    public List<OrderByIdItems> OrderItems { get; set; }
}
