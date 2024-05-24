using Microsoft.AspNetCore.SignalR.Client;
using SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    endpoints.MapHub<ChatHub>("/chathub");
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();
