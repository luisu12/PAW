using PAW.MinimalAPI.Services;
using PAW.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IPawTask2ExternalApiClient, PawTask2ExternalApiClient>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var pawTask2Items = app.MapGroup("/pawtask2items").WithOpenApi();

pawTask2Items.MapGet("/", async (IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var items = await apiClient.GetAllAsync(ct);
    return Results.Ok(items);
});

pawTask2Items.MapGet("/complete", async (IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var items = await apiClient.GetCompletedAsync(ct);
    return Results.Ok(items);
});

pawTask2Items.MapGet("/{id:int}", async (int id, IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var item = await apiClient.GetByIdAsync(id, ct);
    return item is null ? Results.NotFound() : Results.Ok(item);
});

pawTask2Items.MapPost("/", async (PawTask2DTO pawTask2Item, IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var created = await apiClient.CreateAsync(pawTask2Item, ct);
    return Results.Created($"/pawtask2items/{created.Id}", created);
});

pawTask2Items.MapPut("/{id:int}", async (int id, PawTask2DTO pawTask2Item, IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var updated = await apiClient.UpdateAsync(id, pawTask2Item, ct);
    return updated ? Results.NoContent() : Results.NotFound();
});

pawTask2Items.MapPatch("/{id:int}", async (int id, PawTask2DTO pawTask2Item, IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var updated = await apiClient.PatchAsync(id, pawTask2Item, ct);
    return updated ? Results.NoContent() : Results.NotFound();
});

pawTask2Items.MapDelete("/{id:int}", async (int id, IPawTask2ExternalApiClient apiClient, CancellationToken ct) =>
{
    var deleted = await apiClient.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();
