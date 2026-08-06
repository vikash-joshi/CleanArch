using FluentValidation;
using MediatR;
using ProductManagement.Application.Behaviours;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Commands;
using ProductManagement.Application.EventHanlders;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Strategies;
using ProductManagement.Application.Validators;
using ProductManagement.Domain.Common;
using ProductManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
builder.Services.AddScoped<IProductRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Products);
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
app.MapControllers();
app.Run();
