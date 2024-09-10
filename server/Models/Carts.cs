namespace server.Models;

public class Carts
{
    /// <summary>
    /// 購物車編號
    /// </summary>
    public int CartId { get; set; }
    /// <summary>
    /// 使用者編號
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 商品編號
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// 商品數量
    /// </summary>
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}