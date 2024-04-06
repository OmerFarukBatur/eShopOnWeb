namespace BlazorShared.Models;
public class OrderByIdItems
{
    public int Id { get; set; }
    public int CatalogId { get; set; }
    public string ProductName { get; set; }
    public string PictureUri { get; set; }
    public decimal UnitPrice { get; set; }
    public int Unit { get; set; }
    public int OrderId { get; set; }
    public decimal Total { get; set; }
}
