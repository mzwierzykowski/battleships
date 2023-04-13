using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Warships.API.Models;
using Warships.API.Models.Mapping;
using Warships.API.Validators;
using Warships.API.Validators.Abstract;
using Warships.Configuration;
using Warships.Game.DI;
using Warships.Game.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<BoardDimension>(builder.Configuration.GetSection("BoardDimension"));
builder.Services.Configure<FleetConfiguration>(builder.Configuration.GetSection("FleetConfiguration"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IRequestValidator, RequestValidator>();
builder.Services.AddGameServices();
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(builder =>
builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapGet("/game", (IMapper mapper, IGameService gameService) => {
    var gameState = gameService.StartGame();
    var result = mapper.Map<GameState>(gameState);
    return result;
});

app.MapPost("/game", (Request request, IRequestValidator requestValidator, IMapper mapper, IGameService gameService) => {
    if (requestValidator.IsValid(request))
    {
        var gameState = gameService.ShootsFired(request.PointId);
        var result = mapper.Map<GameState>(gameState);
        return Results.Ok(result);
    }
    else
    {
        return Results.BadRequest("Requested point was not in a correct format");
    }
});

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }
