
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var assets = CreateAssets().ToDictionary(a => a.id);

app.UseHttpsRedirection();

app.MapPost("/login", ([Microsoft.AspNetCore.Mvc.FromBody]LoginParameters req) => {
    return TypedResults.Ok(new LoginResult(true, "", "-- TOKEN --"));
}).WithName("Login").WithOpenApi();

app.MapPost("/logout", () => {
    return Results.Ok();
}).WithName("logout").WithOpenApi();



app.MapGet("/loginSalt", ([Microsoft.AspNetCore.Mvc.FromBody]LoginSaltRequest req) => {
    return TypedResults.Ok( "this is the salt for " + req.username);
}).WithName("LoginSalt").WithOpenApi();

app.MapGet("/register", ([Microsoft.AspNetCore.Mvc.FromBody]RegisterUserRequest req) => {
    return TypedResults.Ok();
}).WithName("Register a User").WithOpenApi();


app.MapGet("/assets", () => {
    return TypedResults.Ok(assets.Values);
}).WithName("List Assets").WithOpenApi();


app.MapGet("/assets/{id}", (string id) => {
    
    return TypedResults.Ok(assets[id]);
}).WithName("Get one Asset").WithOpenApi();

app.MapPost("/assets", (Asset asset) =>
{
    if (assets.ContainsKey(asset.id))
    {
        return Results.BadRequest("Asset already exists");
    }
    assets.Add(asset.id, asset);
    return Results.Ok();
}).WithName("Create Asset").WithOpenApi();

app.MapPut("/assets", (Asset asset) =>
{
    if (!assets.ContainsKey(asset.id))
    {
        return Results.NotFound();
    }
    assets[asset.id] = asset;
    return Results.Ok(asset);
}).WithName("Update Asset").WithOpenApi();

app.MapDelete("/assets/{id}", (string id) =>
{
    if (!assets.ContainsKey(id))
    {
        return Results.NotFound();
    }
    assets.Remove(id);
    return Results.Ok();
}).WithName("Delete Asset").WithOpenApi();

app.Run();

static List<Asset> CreateAssets()
{
    var assets = new List<Asset>();
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Tree", "A small tree", "/assets/tree.model", "Clemens", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Rock", "A large rock", "/assets/rock.model", "Eleanor", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Sword", "A sharp sword", "/assets/sword.model", "Gideon", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Shield", "A sturdy shield", "/assets/shield.model", "Isabella", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Potion", "A healing potion", "/assets/potion.model", "Felix", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Castle", "A majestic castle", "/assets/castle.model", "Sophia", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Dragon", "A fearsome dragon", "/assets/dragon.model", "Maximus", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Gem", "A precious gem", "/assets/gem.model", "Aria", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Scroll", "A magical scroll", "/assets/scroll.model", "Darius", GetCurrentDate()));
    assets.Add(new Asset(Guid.NewGuid().ToString(), "Helmet", "A protective helmet", "/assets/helmet.model", "Luna", GetCurrentDate()));
    return assets;
}

static string GetCurrentDate()
{
    return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
}

record LoginParameters(string username, string passwordHash);
record LoginResult(bool success, string message, string token);
record LoginSaltResponse(string salt);
record LoginSaltRequest(string username);
record RegisterUserRequest(string username, string passwordHash, string email);
record Asset(string id, string name, string description, string path, string owner, string created);

