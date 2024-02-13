using BLL.Services.Interfaces;
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
    
    [HttpGet]
    public async Task<IActionResult> CreateBot()
    {
        var file = await _telegramBotService.CreateBot();
        return File(file, "application/zip", "bot.zip");
    }
}