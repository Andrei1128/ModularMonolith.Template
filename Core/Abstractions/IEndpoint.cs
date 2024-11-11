namespace Core.Abstractions;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
