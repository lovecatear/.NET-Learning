using System.Runtime.CompilerServices;

namespace AppSettingsReader;

/// <summary>
/// 用於讀取應用程式設定 (appsettings.json) 中的設定值的類別。
/// </summary>
public class AppSettingsReader
{
    private readonly ILogger<AppSettingsReader> _logger;
    private readonly IConfiguration _config;

    /// <summary>
    /// 初始化 AppSettingsReader 類別的新執行個體。
    /// </summary>
    /// <param name="logger">日誌記錄器</param>
    /// <param name="config">設定配置</param>
    public AppSettingsReader(ILogger<AppSettingsReader> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// 嘗試從設定中獲取值，如果發生錯誤，則返回預設值。
    /// </summary>
    /// <typeparam name="T">要解析的值的類型</typeparam>
    /// <param name="parseFunc">值解析函數</param>
    /// <param name="defaultTValueFunc">預設值函數</param>
    /// <param name="logger">日誌記錄器</param>
    /// <param name="config">設定配置</param>
    /// <param name="key">應用程式設定的鍵名，默認為呼叫該方法的函數名稱</param>
    /// <param name="supressKey">如果函數名稱與應用程式設定的鍵名不同，可以使用此參數替代</param>
    /// <returns>解析後的值或預設值</returns>
    private static T TryGetValueFromConfig<T>(Func<string, T> parseFunc, Func<T> defaultTValueFunc, ILogger<AppSettingsReader> logger, IConfiguration config, [CallerMemberName] string key = "", string supressKey = "")
    {
        try
        {
            string a = string.Empty;

            if (!string.IsNullOrEmpty(supressKey))
            {
                key = supressKey;
            }

            var node = config[key];
            return !string.IsNullOrEmpty(node) ? parseFunc(node) : defaultTValueFunc();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading appsettings.json on {key} node.", key);
            return default;
        }
    }

    /// <summary>
    /// 從應用程式設定中讀取圖片路徑。
    /// </summary>
    public string ImagePath
    {
        get
        {
            return TryGetValueFromConfig(_ => _, () => "FileSystemImageProvider", _logger, _config, supressKey: "ImageProvider");
        }
    }

    /// <summary>
    /// 檢查應用程式設定中是否存在圖片路徑設定。
    /// </summary>
    public bool IsImagePathExists
    {
        get
        {
            return TryGetValueFromConfig(bool.Parse, () => true, _logger, _config);
        }
    }

    /// <summary>
    /// 從應用程式設定中讀取圖片目錄中的圖片數量。
    /// </summary>
    public int CountImagesInDirectory
    {
        get
        {
            return TryGetValueFromConfig(int.Parse, () => 20, _logger, _config);
        }
    }
}