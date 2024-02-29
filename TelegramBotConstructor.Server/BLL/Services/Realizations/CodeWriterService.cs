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
        switchConstructorBuilder.AppendLine($"switch (message)");
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
    
    public string CreateStatesValueSql(List<TelegramUserState> states)
    {
        var statesValueBuilder = new StringBuilder();
        foreach (var state in states)
        {
            statesValueBuilder.Append($"({state.Id}, '{state.State}'),");
        }
        var statesValue = statesValueBuilder.ToString();
        return statesValue.Remove(statesValue.Length - 1);
    }

    private static string CreateSwitchCase(TelegramAnswerPairModel pairModel)
    {
        var switchBuilder = new StringBuilder();
        switchBuilder.AppendLine(
            pairModel.Message == "default"
            ? "default:"
            : $"case \"{pairModel.Message}\":"
        );
        if(pairModel.Nested.Count != 0)
        {
            switchBuilder.AppendLine("await ChangeUserState(connection, chatId, message);");
        }
        switchBuilder.AppendLine($"answer = \"{pairModel.Answer}\";");
        switchBuilder.AppendLine("break;");

        return switchBuilder.ToString();
    }
}