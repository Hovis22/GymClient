var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddHttpClient(name: "Gym",
           options =>
           {
               options.BaseAddress = new Uri("http://localhost:5033/");
               //options.DefaultRequestHeaders.Accept.Add(
               //new MediaTypeWithQualityHeaderValue(
               //"application/json", 1.0));
               //options.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
           });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
