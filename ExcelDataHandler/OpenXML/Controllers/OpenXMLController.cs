using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using OpenXML.Models;

namespace OpenXML.Controllers
{
    /// <summary>
    /// OpenXML
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "api")]
    [Route("api/[controller]")]
    public class OpenXMLController : Controller
    {
        /// <summary>
        /// 匯入檔案
        /// </summary>
        /// <param name="file">Excel檔案</param>
        /// <param name="hasHeader">是否包含標題行</param>
        /// <returns>包含轉換後資料的JSON結果</returns>
        [HttpPost("importExcel")]
        public IActionResult ImportExcel(IFormFile file, bool hasHeader)
        {
            if (file is not null)
            {
                try
                {
                    // 將Excel工作表轉換為資料表
                    List<ImportExcelData> dataList = WorksheetToTable(file, hasHeader);

                    // 回傳成功結果與資料
                    return Json(new
                    {
                        status = true,
                        dataList
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        status = false,
                        message = ex.Message
                    });
                }
            }

            // 檔案不存在，回傳錯誤訊息
            return Json(new
            {
                status = false,
                message = "檔案錯誤"
            });
        }

        /// <summary>
        /// 將Excel工作表轉換為表格資料
        /// </summary>
        /// <param name="excel">Excel檔案</param>
        /// <param name="hasHeader">是否包含標題行</param>
        /// <returns>包含轉換後資料的List</returns>
        private static List<ImportExcelData> WorksheetToTable(IFormFile excel, bool hasHeader)
        {
            // 建立空的會計科目資料表
            List<ImportExcelData> dataList = new();

            // 使用Open XML打開Excel文件
            using (var stream = excel.OpenReadStream())
            using (var spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
            {
                // 獲取工作簿的部分
                var workbookPart = spreadsheetDocument.WorkbookPart ?? throw new Exception("Workbook part not found.");

                // 獲取工作表
                var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault() ?? throw new Exception("No sheets found in the workbook.");

                // 獲取工作表的部分
                var worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart ?? throw new Exception("Worksheet part not found.");
                var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                // 獲取所有行
                var rows = sheetData.Elements<Row>();

                // 跳過標題行
                foreach (var row in hasHeader ? rows.Skip(1) : rows)
                {
                    // 獲取所有儲存格
                    var cells = row.Elements<Cell>().ToList();

                    // 創建 Model 並填充屬性
                    var data = new ImportExcelData();

                    // 檢查 Code 是否符合驗證
                    string? cellValue = GetCellValue(cells[0], workbookPart);
                    if (cellValue is null || cellValue.Length > 1)
                    {
                        data.Match = false;
                    }
                    else if (new[] { "0", "R", "A", "B", "M", "P", "S", "T", "W" }.Contains(cellValue))
                    {
                        data.Code = cellValue;
                    }
                    else
                    {
                        data.Match = false;
                    }

                    // 檢查 IsEnabled 是否符合驗證
                    cellValue = GetCellValue(cells[1], workbookPart);
                    if (bool.TryParse(cellValue, out bool incomeValue))
                    {
                        data.IsEnabled = incomeValue;
                    }
                    else
                    {
                        data.Match = false;
                    }

                    dataList.Add(data);
                }
            }

            return dataList;
        }

        /// <summary>
        /// 從Excel儲存格獲取值
        /// </summary>
        /// <param name="cell">儲存格</param>
        /// <returns>儲存格中的值</returns>
        private static string? GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue is null) ? cell.InnerText : cellValue.Text;

            if (cell.DataType is not null && cell.DataType.Value == CellValues.SharedString)
            {
                var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                return sharedStringTable.ElementAt(int.Parse(text)).InnerText;
            }

            return string.IsNullOrEmpty(text) ? null : text;
        }
    }
}