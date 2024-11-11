using Microsoft.AspNetCore.JsonPatch;

namespace Core.Extensions;

public static class JsonPatchDocumentExtensions
{
    public static bool TryGetValue<T>(this JsonPatchDocument<T> patchDoc, string path, out object? value) where T : class
    {
        value = patchDoc.Operations.FirstOrDefault(o => string.Equals(o.path.ToString(), $"/{path}", StringComparison.OrdinalIgnoreCase))?.value;
        return value != null;
    }
}
