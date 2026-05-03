var builder = WebApplication.CreateBuilder(args);

// Tell the app to use Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Setup error handling for different environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Basic web server settings
app.UseHttpsRedirection();
app.UseStaticFiles(); // Lets the app use CSS and JS files
app.UseRouting();
app.UseAuthorization();

// Map our page files to web routes
app.MapRazorPages();

// When someone opens the root URL, send them straight to the Index page
app.MapGet("/", context => {
    context.Response.Redirect("/Index");
    return Task.CompletedTask;
});

app.Run();