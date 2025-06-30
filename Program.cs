using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);



// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



builder.Services.AddScoped<SpaceMarineAPI.Repositories.UserRepository>();
builder.Services.AddScoped<SpaceMarineAPI.Services.UserService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<SpaceMarineAPI.Repositories.UserRepository>();
builder.Services.AddScoped<SpaceMarineAPI.Services.UserService>();

builder.Services.AddScoped<SpaceMarineAPI.Repositories.SquadRepository>();
builder.Services.AddScoped<SpaceMarineAPI.Services.SquadService>();



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()   // potentionally restrict this later
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();



app.UseCors(MyAllowSpecificOrigins);



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
