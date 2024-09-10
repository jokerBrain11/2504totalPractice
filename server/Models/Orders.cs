namespace server.Models;

public class Orders
{
    /// <summary>
    /// 訂單編號
    /// </summary>
    public int OrderId { get; set; }
    /// <summary>
    /// 使用者編號
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 訂單總金額
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// 訂單狀態
    /// </summary>
    public string OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}