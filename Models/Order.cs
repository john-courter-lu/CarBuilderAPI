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
}

