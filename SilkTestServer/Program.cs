


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

app.UseHttpsRedirection();


app.MapPost("/login", ([Microsoft.AspNetCore.Mvc.FromBody]LoginParameters req) => {
    return Results.Json( new LoginResult(true, "", "-- TOKEN --"));
}).WithName("Login").WithOpenApi();

app.MapGet("/loginSalt", ([Microsoft.AspNetCore.Mvc.FromBody]LoginSaltRequest req) => {
    return Results.Json( new LoginSaltResponse("this is the salt for " + req.username));
}).WithName("LoginSalt").WithOpenApi();

app.Run();

record LoginParameters(string username, string passwordHash);
record LoginResult(bool success, string message, string token);
record LoginSaltResponse(string salt);
record LoginSaltRequest(string username);