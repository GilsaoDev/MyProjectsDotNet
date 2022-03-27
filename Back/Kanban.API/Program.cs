using Kanban.API;
using Kanban.API.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Kanban.API.ApiEndpoints;
using Kanban.API.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

#region Adicionar Services do container //--> método ConfigureServices() 
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();
#endregion


var app = builder.Build();

#region Configurar o HTTP request pipeline. //--> método Configure() 
app.MapAutenticacaoEndpoints();
app.MapCardsEndpoints();

var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();
#endregion

app.UseAuthentication();
app.UseAuthorization();


app.Run();
