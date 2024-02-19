using System.Text;
using BLL.Services.Interfaces;
using Configurations;
using Domain.Models;

namespace BLL.Services.Realizations;

public class CodeWriterService : ICodeWriterService
{
    public string CreateSwitchConstructor(List<TelegramAnswerPairModel> answerPairs)
    {
        var switchConstructorBuilder = new StringBuilder();
        switchConstructorBuilder.AppendLine($"switch ({CodeWriterConfiguration.MessageTextVariableName})");
        switchConstructorBuilder.AppendLine("{");
        
        foreach (var pair in answerPairs)
        {
            switchConstructorBuilder.AppendLine(CreateSwitchCase(pair));
        }
        
        switchConstructorBuilder.AppendLine("}");

        return switchConstructorBuilder.ToString();
    }

    public string CreateButtons(List<TelegramAnswerPairModel> answerPairs)
    {
        var buttonsBuilder = new StringBuilder();
        foreach (var pair in answerPairs.Where(pair => pair.Button))
        {
            buttonsBuilder.AppendLine($$"""new KeyboardButton[] {"{{pair.Message}}"},""");
        }

        return buttonsBuilder.ToString();
    }

    private static string CreateSwitchCase(TelegramAnswerPairModel pairModel)
    {
        var switchBuilder = new StringBuilder();
        switchBuilder.AppendLine(
            pairModel.Message == "default"
            ? "default:"
            : $"case \"{pairModel.Message}\":"
        );
        switchBuilder.AppendLine($"answer = \"{pairModel.Answer}\";");
        switchBuilder.AppendLine("break;");

        return switchBuilder.ToString();
    }
}