using System.Text.Json.Serialization;
using CarBuilderAPI.Models;
using Microsoft.AspNetCore.Http.Json;

// Paint Colors
List<PaintColor> paintColors = new List<PaintColor>
{
    new PaintColor { Id = 1, Color = "Silver", Price = 500 },
    new PaintColor { Id = 2, Color = "Midnight Blue", Price = 600 },
    new PaintColor { Id = 3, Color = "Firebrick Red", Price = 550 },
    new PaintColor { Id = 4, Color = "Spring Green", Price = 450 }
};

// Interior Options
List<Interior> interiors = new List<Interior>
{
    new Interior { Id = 1, Material = "Beige Fabric", Price = 300 },
    new Interior { Id = 2, Material = "Charcoal Fabric", Price = 350 },
    new Interior { Id = 3, Material = "White Leather", Price = 700 },
    new Interior { Id = 4, Material = "Black Leather", Price = 750 }
};

// Technology Packages
List<Technology> technologies = new List<Technology>
{
    new Technology { Id = 1, Price = 1000, Package = "Basic Package (basic sound system)" },
    new Technology { Id = 2, Price = 1500, Package = "Navigation Package (includes integrated navigation controls)" },
    new Technology { Id = 3, Price = 1200, Package = "Visibility Package (includes side and rear cameras)" },
    new Technology { Id = 4, Price = 2500, Package = "Ultra Package (includes navigation and visibility packages)" }
};

// Wheels Options
List<Wheels> wheels = new List<Wheels>
{
    new Wheels { Id = 1, Style = "17-inch Pair Radial", Price = 400 },
    new Wheels { Id = 2, Style = "17-inch Pair Radial Black", Price = 450 },
    new Wheels { Id = 3, Style = "18-inch Pair Spoke Silver", Price = 500 },
    new Wheels { Id = 4, Style = "18-inch Pair Spoke Black", Price = 550 }
};

// Orders Collection (You can leave this empty for now)
List<Order> orders = new List<Order>
{
    new Order
    {
        Id = 1,
        Timestamp = DateTime.Now.AddDays(-3),
        WheelId = 2,
        TechnologyId = 3,
        PaintId = 1,
        InteriorId = 4
    },
    new Order
    {
        Id = 2,
        Timestamp = DateTime.Now.AddDays(-2),
        WheelId = 1,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 2
    },
    new Order
    {
        Id = 3,
        Timestamp = DateTime.Now.AddDays(-1),
        WheelId = 4,
        TechnologyId = 4,
        PaintId = 2,
        InteriorId = 3
    },
    new Order
    {
        Id = 4,
        Timestamp = DateTime.Now.AddHours(-12),
        WheelId = 3,
        TechnologyId = 1,
        PaintId = 4,
        InteriorId = 1
    },
    new Order
    {
        Id = 5,
        Timestamp = DateTime.Now.AddHours(-6),
        WheelId = 1,
        TechnologyId = 3,
        PaintId = 2,
        InteriorId = 4
    }
};


/* 

// Now you have populated collections for paint colors, interiors, technologies, and wheels.
// You can work with these collections to build your CarBuilderAPI logic.

// Example: Accessing a paint color's price
// decimal silverPaintPrice = paintColors.Find(p => p.Color == "Silver")?.Price ?? 0;
// if the Find operation doesn't find a "Silver" paint color, or if the found paint color doesn't have a Price property (due to being null), the silverPaintPrice will be assigned the value of 0. If the Price property exists and is not null, then the value of that property will be assigned to silverPaintPrice.
// Console.WriteLine($"Silver Paint Price: {silverPaintPrice}");

// Example: Accessing an interior's material
// string charcoalFabricMaterial = interiors.Find(i => i.Material == "Charcoal Fabric")?.Material;
// Console.WriteLine($"Charcoal Fabric Material: {charcoalFabricMaterial}");

// Example: Accessing a technology package's ID
// int visibilityPackageId = technologies.Find(t => t.Package.Contains("Visibility Package"))?.Id ?? -1;
// Console.WriteLine($"Visibility Package ID: {visibilityPackageId}"); 

*/

//在上面增加List

//下面是模板
var builder = WebApplication.CreateBuilder(args);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Set the JSON serializer options end

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add CORS, Cross-Origin Resource Sharing
builder.Services.AddCors();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // add Cors
    app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
    // Our API is now telling the browser that it is safe to request the data from any origin.
}

app.UseHttpsRedirection();
//模板结束. 上面的code是设置一个app来be served as a web API

//下面, 创建endpoint ; creat basic endpoints needed below;
app.MapGet("/wheels", () =>
{
    return Results.Ok(wheels);
});

app.MapGet("/technologies", () =>
{
    return Results.Ok(technologies);
});

app.MapGet("/interiors", () =>
{
    return Results.Ok(interiors);
});

app.MapGet("/paintcolors", () =>
{
    return Results.Ok(paintColors);
});

app.MapGet("/orders", () =>
{
    List<Order> ordersToBeFulfilled = orders.FindAll(order =>
    order.DateFulfilled == null);
    // 也可以用.Where 但是 要加.ToList()

    List<Order> ordersWithDetails = ordersToBeFulfilled.Select(order =>
    {
        return new Order
        {
            Id = order.Id,
            Timestamp = order.Timestamp,
            WheelId = order.WheelId,
            TechnologyId = order.TechnologyId,
            PaintId = order.PaintId,
            InteriorId = order.InteriorId,
            Wheels = wheels.FirstOrDefault(w => w.Id == order.WheelId),
            Technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId),
            Paint = paintColors.FirstOrDefault(p => p.Id == order.PaintId),
            Interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId),
            //TotalCost = order.TotalCost 
            //这是会带来compiler error的, 因为 calculated properties is read-only. 这也是不需要的, 因为会自动计算, 会根据the getter of the TotalCost property 里面的logic
            DateFulfilled = order.DateFulfilled
            //这要是没有的话, 用 fetch get 或http get的order的DateFulfilled都是初始值/null.
        };
    }).ToList();

    return Results.Ok(ordersWithDetails);
});
// 点评: .Select等于 .map 或 .ForEach
// .ToList()等于在.ForEach中加入开始 var orderWithDetails = new Order; 最后ordersWithDetails.Add(orderWithDetails);

app.MapPost("/orders", (Order newOrder) =>
{
    // Create a new id
    newOrder.Id = orders.Count > 0 ? orders.Max(order => order.Id) + 1 : 1;

    // Set the timestamp to the current server time
    newOrder.Timestamp = DateTime.Now;

    // Add the new order to the orders collection
    orders.Add(newOrder);

    return Results.Ok(newOrder);
});

app.MapPost("/orders/{orderId}/fulfill", (int orderId) =>
{

    Order orderToFulfill = orders.FirstOrDefault(obj => obj.Id == orderId);

    //if (orderToFulfill == null)
    //{ return Results.NotFound(); }

    orderToFulfill.DateFulfilled = DateTime.Now;

    //return Results.Ok(orderToFulfill);

});
// 注意是{orderId} 而不是${orderId}, 没有 $
// 不确定是否要最后的return, 因为HoneyRaesAPI中没有return. 但是要在if中加入return 就必须在最后也加入return, 否则compiler error.
// 我决定放弃. 我不知道为什么. 
// 我可以在HoneyRaesAPI中, 再试试. 
// 问题: 为何有些MapPost需要return, 有些不需要呢.要是加入return,则会返回Error: response status is 500

app.Run();
