using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RogueStarIdle.PlugIns.InMemory;
using RogueStarIdle.ServerApplication.Data;
using RogueStarIdle.ServerApplication.Pages;
using RogueStarIdle.ServerApplication.Shared.State;
using RogueStarIdle.UseCases.Items;
using RogueStarIdle.UseCases.Items.Interfaces;
using RogueStarIdle.UseCases.Items.PluginInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();
builder.Services.AddTransient<IViewItemsByNameUseCase, ViewItemsByNameUseCase>();
builder.Services.AddTransient<IViewItemsByTagUseCase, ViewItemsByTagUseCase>();
builder.Services.AddScoped<EquipmentState>();
builder.Services.AddScoped<InventoryState>();
builder.Services.AddScoped<CharacterState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
