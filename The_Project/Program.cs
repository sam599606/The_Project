using System;
using System.IO;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using The_Project.Data;
using The_Project.Services;

// 獲取根目錄路徑(包含應用程式執行檔案的目錄)
var rootDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
Console.WriteLine($"Root Directory: {rootDirectory}");

// 配置 ASP.NET Core 應用程式
var builder = WebApplication.CreateBuilder(args);

// 創建生成器並設置基本路徑為根目錄、添加 appsettings.json 文件
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(rootDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// 創建配置對象
var configuration = configurationBuilder.Build();

// 向服務容器中添加 MVC 控制器服務
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<MembersDBService>();
builder.Services.AddSingleton<MailService>();

// 添加 Endpoints API Explorer 服務、Swagger 生成器服務
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 創建應用程式對象
var app = builder.Build();

// Configure the HTTP request pipeline.
// 配置開發階段的 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

// 啟用靜態檔案的服務
app.UseStaticFiles();

// 設定 CORS
app.UseCors(builder => builder
    .WithOrigins("http://127.0.0.1:5555", "http://127.0.0.1:5500") // 允許的源網址
    .AllowAnyMethod() // 允許任何 HTTP 方法
    .AllowAnyHeader() // 允許任何標頭
    .AllowCredentials()); // 允許傳送身分驗證 cookie

// 啟用 HTTPS 重新導向
app.UseHttpsRedirection();

// 啟用路由
app.UseRouting();

// 啟用身份驗證和授權
app.UseAuthentication();
app.UseAuthorization();

// 將請求映射到控制器
app.MapControllers();

// 啟動應用程式
app.Run();
