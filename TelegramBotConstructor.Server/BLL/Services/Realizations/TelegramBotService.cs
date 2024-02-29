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
            MakeBotCode(pairModels));
        await _fileStorageService.BuildProject(Path.Combine(workingDirectory, randomProjectName));
        
        var archivePath = Path.Combine(projectPath, $"{randomProjectName}.zip");
        return await _fileStorageService.CreateArchiveFromFolder(projectPath, archivePath);
    }
    
    public List<TelegramUserState> GetStates(List<TelegramAnswerPairModel> pairModels)
    {
        var states = new List<TelegramUserState>();
        foreach (var pairModel in pairModels)
        {
            states.AddRange(GetNestedStates(pairModel));
        }
        
        AddIdToStates(states);
        return states;
    }
    
    

    private static IEnumerable<TelegramUserState> GetNestedStates(TelegramAnswerPairModel pairModel)
    {
        var states = new List<TelegramUserState>();
        if (pairModel.Nested.Count == 0) return states;
        
        states.Add(new()
        {
            State = pairModel.Message
        });
        foreach (var nested in pairModel.Nested)
        {
            states.AddRange(GetNestedStates(nested));
        }

        return states;
    }
    
    private static void AddIdToStates(IReadOnlyList<TelegramUserState> states)
    {
        for (var i = 0; i < states.Count; i++)
        {
            states[i].Id = i + 1;
        }
    } 

    private string MakeBotCode(List<TelegramAnswerPairModel> pairModels)
    {
        var botText = File.ReadAllText(Path.Combine(ProjectSavingPlaceConfiguration.BaseProjectPath,"Program.cs"));
        botText = botText.Replace("// {{buttons}}", _codeWriterService.CreateButtons(pairModels));
        botText = botText.Replace("// {{innerCode}}", _codeWriterService.CreateSwitchConstructor(pairModels));
        botText = botText.Replace("// {{states}}", _codeWriterService.CreateStatesValueSql(GetStates(pairModels)));
        return botText;
    }
}