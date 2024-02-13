namespace BLL.Services.Interfaces;

public interface ITelegramBotService
{
    public Task<FileStream> CreateBot();
}