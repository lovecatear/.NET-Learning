using System.Reflection;

var before = new SiteDto { Name = "Google", Server = "8.8.8.8" };
var after = new SiteDto { Name = "Hinet", Server = "168.95.1.1" };
Console.WriteLine(DetailedCompare(before, after));

/// <summary>
/// 比較兩個泛型物件的差異並返回差異的詳細信息
/// </summary>
/// <typeparam name="T">要比較的物件類型</typeparam>
/// <param name="before">比較之前的物件</param>
/// <param name="after">比較之後的物件</param>
/// <returns>包含差異信息的字符串</returns>
static string DetailedCompare<T>(T before, T after) where T : new()
{
    // 如果兩個物件都為 null，則返回空字符串
    if (before == null && after == null) return string.Empty;

    // 如果其中一個物件為 null，則初始化為新物件
    before ??= new T();
    after ??= new T();

    // 獲取泛型參數類型的屬性信息
    Type type = typeof(T);
    var variances = new List<string>();

    // 獲取該類型的所有公共實例屬性
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    // 遍歷每個屬性，比較其值
    foreach (PropertyInfo property in properties)
    {
        // 獲取屬性的值，如果為 null 則設置為空字符串
        var beforeValue = property.GetValue(before) ?? string.Empty;
        var afterValue = property.GetValue(after) ?? string.Empty;

        // 如果屬性值不相等，則將差異信息添加到列表中
        if (!beforeValue.Equals(afterValue))
            variances.Add($"{property.Name}: {beforeValue} => {afterValue}");
    }

    // 返回所有差異信息的字符串表示形式，每個差異信息一行
    return string.Join(Environment.NewLine, variances);
}

public class SiteDto
{
    public string Name { get; set; } = "defaultName";

    public string Server { get; set; } = "defaultServer";
}