using BLL.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotBuilderController : ControllerBase
{
    private readonly ITelegramBotService _telegramBotService;

    public BotBuilderController(ITelegramBotService telegramBotService)
    {
        _telegramBotService = telegramBotService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBot(TelegramSettingsModel telegramSettingsModel)
    {
        var file = await _telegramBotService.CreateBot(telegramSettingsModel.MessageAnswers);
        return File(file, "application/zip", "bot.zip");
    }
}