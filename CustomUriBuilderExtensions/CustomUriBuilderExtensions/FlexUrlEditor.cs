using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace CustomUriBuilderExtensions;

// 自定義的 UriBuilder 擴充類別
public class FlexUrlEditor : UriBuilder
{
    private readonly NameValueCollection collection; // 存儲查詢參數的集合

    /// <summary>
    /// 初始化 FlexUrlEditor 物件並解析 URL 中的查詢參數。
    /// </summary>
    /// <param name="url">要操作的 URL 字串。</param>
    public FlexUrlEditor(string url) : base(url)
    {
        // 使用 HttpUtility.ParseQueryString 方法解析 URL 中的查詢參數
        // 並將它們存儲在 collection 變數中
        collection = HttpUtility.ParseQueryString(new Uri(url).Query, Encoding.UTF8);
    }

    /// <summary>
    /// 存取查詢參數的值，並允許設定新的值。
    /// </summary>
    /// <param name="key">查詢參數的名稱。</param>
    /// <returns>查詢參數的值。</returns>
    public string this[string key]
    {
        get { return collection[key]; }
        set
        {
            if (value == null && collection[key] != null)
            {
                // 如果新值為 null 且存在相應的查詢參數，則刪除它
                collection.Remove(key);
            }
            else
            {
                // 否則，設定查詢參數的值
                collection[key] = value;
            }
        }
    }

    /// <summary>
    /// 生成編輯後的 URL 字串。
    /// </summary>
    /// <returns>編輯後的 URL 字串。</returns>
    public string GenUrl()
    {
        // 編碼並解碼查詢參數，然後設定為 UriBuilder 的查詢部分 REF: https://goo.gl/gHmGUz
        base.Query = Uri.EscapeUriString(HttpUtility.UrlDecode(collection.ToString()));

        // 如果 URL 使用默認的 HTTP（80）或 HTTPS（443）端口，則移除端口號
        if (base.Uri.IsDefaultPort) base.Port = -1;

        // 返回最終的 URL 字串
        return base.ToString();
    }
}