using BLL.Services.Interfaces;
using Configurations;

namespace BLL.Services.Realizations;

public class TelegramBotService(IFileStorageService fileStorageService) : ITelegramBotService
{
    public async Task<FileStream> CreateBot()
    {
        var randomProjectName = $"Project{Guid.NewGuid()}";
        var workingDirectory = ProjectSavingPlaceConfiguration.ProjectSavingPlace;
        var projectPath = Path.Combine(workingDirectory, randomProjectName);

        fileStorageService.CopyDirectory(
            ProjectSavingPlaceConfiguration.BaseProjectPath, 
            Path.Combine(workingDirectory, randomProjectName), 
            true);
        
        await fileStorageService.ChangeProjectCode(Path.Combine(workingDirectory, randomProjectName), """
            
                                using Telegram.Bot;
                                using Telegram.Bot.Types;
            
                                await new TelegramBotClient("").SendTextMessageAsync(new ChatId(""), "");

            """);
        await fileStorageService.BuildProject(Path.Combine(workingDirectory, randomProjectName));
        
        var archivePath = Path.Combine(projectPath, $"{randomProjectName}.zip");
        return await fileStorageService.CreateArchiveFromFolder(projectPath, archivePath);
    }
}