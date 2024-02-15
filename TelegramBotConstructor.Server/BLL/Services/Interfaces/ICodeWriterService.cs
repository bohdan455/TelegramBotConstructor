using Domain.Models;

namespace BLL.Services.Interfaces;

public interface ICodeWriterService
{
    string CreateSwitchConstructor(List<TelegramAnswerPairModel> answerPairs);
}