using BLL.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotBuilderController : ControllerBase
{
    private readonly ITelegramBotService _telegramBotService;
    private readonly ILogger<BotBuilderController> _logger;

    public BotBuilderController(
        ITelegramBotService telegramBotService,
        ILogger<BotBuilderController> logger)
    {
        _telegramBotService = telegramBotService;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBot(TelegramSettingsModel telegramSettingsModel)
    {
        _logger.LogInformation("Creating bot");
        var file = await _telegramBotService.CreateBot(telegramSettingsModel.MessageAnswers);
        _logger.LogInformation("Bot created");
        
        return File(file, "application/zip", "bot.zip");
    }
}