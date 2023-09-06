using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// ����r�ꫬ�A��ƪ��^�Ǵ���
app.MapGet("/string", (ILogger<AppSettingsReader.AppSettingsReader> logger, IConfiguration config) =>
{
    try
    {
        // Ū���ê�^���ε{���]�w�����Ϥ����|�]�w
        return Results.Json(new AppSettingsReader.AppSettingsReader(logger, config).ImagePath);
    }
    catch (Exception ex)
    {
        return Results.Json(ex.ToString());
    }
});

// ���楬�L���A��ƪ��^�Ǵ���
app.MapGet("/bool", (ILogger<AppSettingsReader.AppSettingsReader> logger, IConfiguration config) =>
{
    try
    {
        // Ū���ê�^���ε{���]�w�������L�ȳ]�w
        return Results.Json(new AppSettingsReader.AppSettingsReader(logger, config).IsImagePathExists);
    }
    catch (Exception ex)
    {
        return Results.Json(ex.ToString());
    }
});

// ����Ʀr���A��ƪ��^�Ǵ���
app.MapGet("/number", (ILogger<AppSettingsReader.AppSettingsReader> logger, IConfiguration config) =>
{
    try
    {
        // Ū���ê�^���ε{���]�w�����Ʀr�ȳ]�w
        return Results.Json(new AppSettingsReader.AppSettingsReader(logger, config).CountImagesInDirectory);
    }
    catch (Exception ex)
    {
        return Results.Json(ex.ToString());
    }
});

app.Run();