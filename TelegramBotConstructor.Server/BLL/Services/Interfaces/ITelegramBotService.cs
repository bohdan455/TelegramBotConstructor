using Domain.Models;

namespace BLL.Services.Interfaces;

public interface ITelegramBotService
{
    Task<Stream> CreateBot(List<TelegramAnswerPairModel> pairModels);
    
    List<TelegramUserState> GetStates(List<TelegramAnswerPairModel> pairModels);
}