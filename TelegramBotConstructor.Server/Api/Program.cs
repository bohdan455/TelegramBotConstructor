using Api.Services;
using BLL.Services.Interfaces;
using BLL.Services.Realizations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IFileStorageService, FileStorageService>();
builder.Services.AddTransient<ITelegramBotService, TelegramBotService>();
builder.Services.AddTransient<ICodeWriterService, CodeWriterService>();
builder.Services.AddHostedService<BaseProjectRestoreService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["Angular"] ?? throw new ArgumentException("Provide Angular URL in appsettings.json"))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();