using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// 執行字串型態資料的回傳測試
app.MapGet("/string", (ILogger<AppSettingsReader.AppSettingsReader> logger, IConfiguration config) =>
{
    try
    {
        // 讀取並返回應用程式設定中的圖片路徑設定
        return Results.Json(new AppSettingsReader.AppSettingsReader(logger, config).ImagePath);
    }
    catch (Exception ex)
    {
        return Results.Json(ex.ToString());
    }
});

// 執行布林型態資料的回傳測試
app.MapGet("/bool", (ILogger<AppSettingsReader.AppSettingsReader> logger, IConfiguration config) =>
{
    try
    {
        // 讀取並返回應用程式設定中的布林值設定
        return Results.Json(new AppSettingsReader.AppSettingsReader(logger, config).IsImagePathExists);
    }
    catch (Exception ex)
    {
        return Results.Json(ex.ToString());
    }
});

// 執行數字型態資料的回傳測試
app.MapGet("/number", (ILogger<AppSettingsReader.AppSettingsReader> logger, IConfiguration config) =>
{
    try
    {
        // 讀取並返回應用程式設定中的數字值設定
        return Results.Json(new AppSettingsReader.AppSettingsReader(logger, config).CountImagesInDirectory);
    }
    catch (Exception ex)
    {
        return Results.Json(ex.ToString());
    }
});

app.Run();