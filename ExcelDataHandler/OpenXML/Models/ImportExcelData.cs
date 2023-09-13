namespace OpenXML.Models;

/// <summary>
/// 匯入檔案 轉成表格
/// </summary>
public class ImportExcelData
{
    /// <summary>
    /// 是否符合格式
    /// </summary>
    public bool Match { get; set; } = true;

    /// <summary>
    /// 代碼
    /// </summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsEnabled { get; set; } = false;
}