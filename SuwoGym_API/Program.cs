var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

var app = builder.Build();


app.UseExceptionHandler("/api/Error/RegistrarError");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
