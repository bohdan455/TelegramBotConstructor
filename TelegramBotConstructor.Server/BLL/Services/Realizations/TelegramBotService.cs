using BLL.Services.Interfaces;
using Configurations;

namespace BLL.Services.Realizations;

public class TelegramBotService : ITelegramBotService
{
    private readonly IFileStorageService _fileStorageService;

    public TelegramBotService(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    public async Task<FileStream> CreateBot()
    {
        var randomProjectName = $"Project{Guid.NewGuid()}";
        var workingDirectory = ProjectSavingPlaceConfiguration.ProjectSavingPlace;
        var projectPath = Path.Combine(workingDirectory, randomProjectName);

        _fileStorageService.CopyDirectory(
            ProjectSavingPlaceConfiguration.BaseProjectPath, 
            Path.Combine(workingDirectory, randomProjectName), 
            true);
        
        await _fileStorageService.ChangeProjectCode(Path.Combine(workingDirectory, randomProjectName), """
            
                                using Telegram.Bot;
                                using Telegram.Bot.Types;
            
                                await new TelegramBotClient("").SendTextMessageAsync(new ChatId(""), "");

            """);
        await _fileStorageService.BuildProject(Path.Combine(workingDirectory, randomProjectName));
        
        var archivePath = Path.Combine(projectPath, $"{randomProjectName}.zip");
        return await _fileStorageService.CreateArchiveFromFolder(projectPath, archivePath);
    }

    private static string GetBotCode(string innerCode)
    {
        var code = """
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var builder = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(),"appsettings.json"), optional: false);

IConfiguration config = builder.Build();

var token = config["BotToken"] ?? throw new ArgumentException("Provide bot token in appsettings.json");

var botClient = new TelegramBotClient(token);

using CancellationTokenSource cts = new ();

ReceiverOptions receiverOptions = new ()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
        if (update.Message is not { Text: { } message } messageObject)
            return;

        var chatId = messageObject.Chat.Id;

        Console.WriteLine($"Received a '{message}' message in chat {chatId}.");
        var answer = "Invalid message";
    
        var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: answer,
                cancellationToken: cancellationToken);
            }

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
            }
""";

        return code;
    }
}