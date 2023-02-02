using CatalogoMinimalAPI.Endpoints;
using CatalogoMinimalAPI.Extensions;



var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistenceJwt();
builder.Services.AddCors();
builder.AddAuthenticationJwt();
    
var app = builder.Build();

app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

var environment = app.Environment;
app.UseExecptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();

