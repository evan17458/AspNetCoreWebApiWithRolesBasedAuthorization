using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class CustomSwaggerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // 定義您想要的順序
        var customOrder = new List<string>
        {
            "TouristRoutes",
            "TouristRoutePictures",
            "Account",

        };

        // 建立一個新的路徑字典，按照自訂順序排列
        var orderedPaths = new OpenApiPaths();
        foreach (var tag in customOrder)
        {
            // 找到該 tag 對應的所有路徑
            var pathsForTag = swaggerDoc.Paths
                .Where(p => p.Value.Operations.Values.Any(op => op.Tags.Any(t => t.Name == tag)))
                .OrderBy(p => p.Key); // 可選擇是否對路徑內部再排序

            foreach (var path in pathsForTag)
            {
                orderedPaths.Add(path.Key, path.Value);
            }
        }

        // 將未包含在自訂順序中的其他路徑加到最後
        var remainingPaths = swaggerDoc.Paths
            .Where(p => !orderedPaths.ContainsKey(p.Key))
            .OrderBy(p => p.Key);

        foreach (var path in remainingPaths)
        {
            orderedPaths.Add(path.Key, path.Value);
        }

        // 替換原始路徑
        swaggerDoc.Paths = orderedPaths;
    }
}