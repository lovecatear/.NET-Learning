using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CustomUriBuilderExtensions;

/// <summary>
/// 用於處理 URL 操作的控制器
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RouteController : ControllerBase
{
    /// <summary>
    /// HTTP GET 請求，用於獲取編輯後的 URL
    /// </summary>
    /// <returns>包含生成的 URL 的 JSON 响應</returns>
    [HttpGet]
    public IActionResult Get()
    {
        // 使用 HttpContext.Request.GetDisplayUrl() 獲取目前請求的 URL
        var builder = new FlexUrlEditor(HttpContext.Request.GetDisplayUrl());
        var builder1 = builder.GenUrl();

        // 以下範例演示如何使用 FlexUrlEditor 來修改和刪除查詢參數
        var url = "http://blog.darkthread.net/?a=1&b=2&c=3";
        builder = new FlexUrlEditor(url);
        builder["b"] = null; // 設定查詢參數 "b" 的值為 null，這將導致它被刪除
        builder["c"] = "中文"; // 設定查詢參數 "c" 的值為 "中文"，這將對值進行編碼
        var builder2 = builder.GenUrl();

        // 創建一個新的 FlexUrlEditor，並設定 URL 的協定為 "http"，端口為 8888
        builder = new FlexUrlEditor("https://blog.darkthread.net")
        {
            Scheme = "http",
            Port = 8888
        };
        var builder3 = builder.GenUrl();

        // 返回包含生成的 URL 的 JSON 响應
        return StatusCode(StatusCodes.Status200OK, new
        {
            builder1,
            builder2,
            builder3
        });
    }
}