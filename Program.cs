
//在上面增加List

//下面是模板
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//模板结束. 上面的code是设置一个app来be served as a web API

//下面, 创建endpoint


app.Run();
