namespace server.Models;

public class OrderDetails
{
    /// <summary>
    /// 訂單明細編號
    /// </summary>
    public int OrderDetailId { get; set; }
    /// <summary>
    /// 訂單編號
    /// </summary>
    public int OrderId { get; set; }
    /// <summary>
    /// 商品編號
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// 商品數量
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// 商品單價
    /// </summary>
    public decimal UnitPrice { get; set; }
}