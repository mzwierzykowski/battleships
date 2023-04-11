using AutoMapper;
using Warships.API.Models;
using Warships.API.Models.Mapping;
using Warships.Configuration;
using Warships.Game.DI;
using Warships.Game.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<BoardDimension>(builder.Configuration.GetSection("BoardDimension"));
builder.Services.Configure<FleetConfiguration>(builder.Configuration.GetSection("FleetConfiguration"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddGameServices();

var app = builder.Build();

app.MapGet("/game", (IMapper mapper, IGameService gameService) => {
    var gameState = gameService.StartGame();
    var result = mapper.Map<GameState>(gameState);
    return TypedResults.Ok(result);
});

app.MapPost("/game",  (string pointId, IMapper mapper, IGameService gameService) => {
    var gameState = gameService.ShootsFired(pointId);
    var result = mapper.Map<GameState>(gameState);
    return TypedResults.Ok(result);
});

app.Run();
