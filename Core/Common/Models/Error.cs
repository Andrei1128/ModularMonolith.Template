namespace Core.Common.Models;

public record Error
{
    protected Error(string code, string description, Type type)
    {
        Code = code;
        Description = description;
        ErrorType = type;
    }

    public string Code { get; }
    public string Description { get; }
    public Type ErrorType { get; }

    public static readonly Error None = new(string.Empty, string.Empty, Type.Failure);
    public static readonly Error NullValue = new("General.Null", "Null value was provided", Type.Failure);

    public static Error Failure(string code, string description) => new(code, description, Type.Failure);
    public static Error NotFound(string code, string description) => new(code, description, Type.NotFound);
    public static Error Problem(string code, string description) => new(code, description, Type.Problem);
    public static Error Conflict(string code, string description) => new(code, description, Type.Conflict);

    public enum Type
    {
        Failure = 0,
        Validation = 1,
        Problem = 2,
        NotFound = 3,
        Conflict = 4
    }
}
