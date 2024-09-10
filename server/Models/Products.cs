namespace server.Models;

public class Products
{
    /// <summary>
    ///  商品編號
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// 商品名稱
    /// </summary>
    public string ProductName { get; set; }
    /// <summary>
    /// 商品描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 商品價格
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// 商品庫存數量
    /// </summary>
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}