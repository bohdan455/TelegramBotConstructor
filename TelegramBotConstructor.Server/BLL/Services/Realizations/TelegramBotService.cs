using BLL.Services.Interfaces;
using Configurations;
using Domain.Models;

namespace BLL.Services.Realizations;

public class TelegramBotService : ITelegramBotService
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICodeWriterService _codeWriterService;

    public TelegramBotService(
        IFileStorageService fileStorageService,
        ICodeWriterService codeWriterService)
    {
        _fileStorageService = fileStorageService;
        _codeWriterService = codeWriterService;
    }
    
    public async Task<Stream> CreateBot(List<TelegramAnswerPairModel> pairModels)
    {
        var randomProjectName = $"Project{Guid.NewGuid()}";
        var workingDirectory = ProjectSavingPlaceConfiguration.ProjectSavingPlace;
        var projectPath = Path.Combine(workingDirectory, randomProjectName);

        _fileStorageService.CopyDirectory(
            ProjectSavingPlaceConfiguration.BaseProjectPath, 
            Path.Combine(workingDirectory, randomProjectName), 
            true);

        await _fileStorageService.ChangeProjectCode(
            Path.Combine(workingDirectory, randomProjectName),
            MakeBotCode(
                _codeWriterService.CreateButtons(pairModels),
                _codeWriterService.CreateSwitchConstructor(pairModels)));
        await _fileStorageService.BuildProject(Path.Combine(workingDirectory, randomProjectName));
        
        var archivePath = Path.Combine(projectPath, $"{randomProjectName}.zip");
        return await _fileStorageService.CreateArchiveFromFolder(projectPath, archivePath);
    }

    private static string MakeBotCode(string buttons,string innerCode)
    {
        var botText = File.ReadAllText(Path.Combine(ProjectSavingPlaceConfiguration.BaseProjectPath,"Program.cs"));
        botText = botText.Replace("// {{buttons}}", buttons);
        botText = botText.Replace("// {{innerCode}}", innerCode);
        return botText;
    }
}