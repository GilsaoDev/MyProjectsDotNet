using Kanban.API;
using Kanban.API.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Kanban.API.ApiEndpoints;
using Kanban.API.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.AddTokenRegister();
builder.Services.AddCors();
builder.AddAutenticationJwt();


var app = builder.Build();

app.MapAutenticacaoEndpoints();
app.MapCardsEndpoints();

var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
