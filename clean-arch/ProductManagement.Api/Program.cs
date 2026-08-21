using System.Text;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

using ProductManagement.Domain.Common;
using ProductManagement.Domain.Factories;

using ProductManagement.Application.Behaviours;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Commands;
using ProductManagement.Application.EventHanlders;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Strategies;
using ProductManagement.Application.Validators;

using ProductManagement.Infrastructure;
using ProductManagement.Infrastructure.Auth;
using ProductManagement.Infrastructure.Decorators;
using ProductManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<InMemoryDatabase>();
builder.Services.AddScoped<IUnitOfWork>(sp => new InMemoryUnitOfWork(
    sp.GetRequiredService<InMemoryDatabase>(),
    sp.GetRequiredService<IDomainEventDispatcher>()));
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var uow = sp.GetRequiredService<IUnitOfWork>();
    var inner = uow.Products;

    var cache = new CachingProductRepository(inner, sp.GetRequiredService<IMemoryCache>());
    var log = new LoggingProductRepository(cache, sp.GetRequiredService<ILogger<LoggingProductRepository>>());

    // replace unit-of-work's product repo with the decorated instance so all consumers
    // (including those that access IUnitOfWork.Products) see the decorator
    uow.Products = log;

    return log;
});
builder.Services.AddScoped<ICategoryRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Categories);
builder.Services.AddScoped<ProductBusinessRules>();
builder.Services.AddScoped<CategoryBusinessRules>();
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();
builder.Services.AddScoped<PricingStrategyFactory>();
builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddScoped<IDomainEventHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
builder.Services.AddScoped<ProductFactory>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/*app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        var (status, title) = ex switch
        {
            DomainException => (StatusCodes.Status400BadRequest, ex.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not found"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new { title, status });
    });
});
*/
app.UseAuthentication();   // must come BEFORE UseAuthorization
app.UseAuthorization();
app.MapControllers();
app.Run();
