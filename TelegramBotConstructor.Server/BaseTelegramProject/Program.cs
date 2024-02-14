using Telegram.Bot;
using Telegram.Bot.Types;

await new TelegramBotClient("").SendTextMessageAsync(new ChatId(""), "If this works - I'm a genius!");