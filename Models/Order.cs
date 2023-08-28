namespace CarBuilderAPI.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
    public Wheels Wheels { get; set; } // Updated property name
    public Technology Technology { get; set; }
    public PaintColor Paint { get; set; }
    public Interior Interior { get; set; }
    public decimal TotalCost
    {
        get
        { return Wheels.Price + Technology.Price + Paint.Price + Interior.Price; }
    }
}

// 注意: 当在Order class中增加了properties,如calculated properties, 要重启dotnet watch run, 因为不会hotload; 所以如果结果不正确, 在修改code前, 重启一下.

// 回答自己的问题: Wheels.Price 不是应该是0吗, 因为Wheels是在endpoint的lambda function 中才被.FirstOrDefault赋值的.
// 答: 因为这是a calculated property, 在每次access it时, 会自动计算. 而在mapGet中access it时, Wheels 已经根据database中的wheels List/Collection和 LinQ method被正确赋值了.
