using Domain.Attributes;

namespace Core.Common.AppSettings;

public class ConnectionStrings
{
    public const string Key = "ConnectionStrings";

    [Decrypt]
    public string SqlServer { get; set; } = string.Empty;
}
