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
}