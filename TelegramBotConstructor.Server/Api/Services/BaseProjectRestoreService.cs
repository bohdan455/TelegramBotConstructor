using BLL.Services.Realizations;
using Configurations;

namespace Api.Services;

public class BaseProjectRestoreService : IHostedService
{
    private readonly ILogger<BaseProjectRestoreService> _logger;
    private readonly IFileStorageService _fileStorageService;

    public BaseProjectRestoreService(ILogger<BaseProjectRestoreService> logger ,IFileStorageService fileStorageService)
    {
        _logger = logger;
        _fileStorageService = fileStorageService;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start restoring base project");
        await _fileStorageService.RestoreProject(ProjectSavingPlaceConfiguration.BaseProjectPath);
        _logger.LogInformation("Base project restored");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}