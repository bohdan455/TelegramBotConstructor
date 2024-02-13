using BLL.Services.Interfaces;

namespace BLL.Services.Realizations;

public class TelegramBotService : ITelegramBotService
{
    private readonly IFileStorageService _fileStorageService;

    public TelegramBotService(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    public Task<FileStream> CreateBot()
    {
        return _fileStorageService.GenerateProjectFiles("Console.WriteLine(\"Hello, bot!\");");
    }
}