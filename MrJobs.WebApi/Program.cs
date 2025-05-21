// Configure the DI container
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenApi();
}

// Configure the HTTP request pipeline
var app = builder.Build();
{
    var isDevelopment = app.Environment.IsDevelopment();
    if (isDevelopment)
    {
        app.MapOpenApi();
    }

    var isProduction = app.Environment.IsProduction();
    if (isProduction)
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.MapGet("/poke", () => "Successfully poked the Web API");

    app.Run();
}
