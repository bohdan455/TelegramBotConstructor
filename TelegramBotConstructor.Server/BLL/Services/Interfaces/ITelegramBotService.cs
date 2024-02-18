using Domain.Models;

namespace BLL.Services.Interfaces;

public interface ITelegramBotService
{
    public Task<Stream> CreateBot(List<TelegramAnswerPairModel> pairModels);
}