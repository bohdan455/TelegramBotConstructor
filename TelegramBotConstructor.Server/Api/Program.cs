using BLL.Services.Interfaces;
using BLL.Services.Realizations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IFileStorageService, FileStorageService>();
builder.Services.AddTransient<ITelegramBotService, TelegramBotService>();
builder.Services.AddTransient<ICodeWriterService, CodeWriterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();