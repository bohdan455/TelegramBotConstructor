using Domain.Models;

namespace BLL.Services.Interfaces;

public interface ITelegramBotService
{
    public Task<FileStream> CreateBot(List<TelegramAnswerPairModel> pairModels);
}