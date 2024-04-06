using System;

namespace BlazorShared.Models;
public class AllOrders
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int OrderId { get; set; }
    public string BuyerId { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public bool Status { get; set; }
    public decimal Total { get; set; }
}
