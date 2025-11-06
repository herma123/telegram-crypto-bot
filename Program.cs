using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using telegramcryptobot;
using telegramcryptobot.src;


internal class Program
{
    private static void Main(string[] args)
    {
        funcrtionsWithBot func = new funcrtionsWithBot();
        RedisDataBase redis = new RedisDataBase();
        using CancellationTokenSource cts = new CancellationTokenSource();
        TelegramBotClient bot = new TelegramBotClient("YOUR TOKEN", cancellationToken: cts.Token);
        bot.OnMessage += OnMessage;
        bot.OnError += OnError;
        Console.ReadLine();
        cts.Cancel();

        async Task OnError(Exception exception, HandleErrorSource source)
        {
            Console.WriteLine(exception);
        }
        async Task OnMessage(Message msg, UpdateType type)
        {
            string[] massive_message = msg.Text.Split(" ");
            switch (massive_message[0])
            {
                case "/start":
                    await bot.SendMessage(msg.Chat, func.start());
                    break;
                case "/genkey":
                    try { await bot.SendMessage(msg.Chat, func.genKey(Convert.ToInt32(massive_message[massive_message.Length - 1]))); }
                    catch { await bot.SendMessage(msg.Chat, func.genKey()); }
                    break;
                case "/encrypt":
                    string data = "";
                    for (int x = 2; x < massive_message.Length; x++) data += massive_message[x] + " ";
                    await bot.SendMessage(msg.Chat, func.encrypt(msg.From.Id.ToString(), massive_message[1], data, redis));
                    break;
                case "/decrypt":
                    await bot.SendMessage(msg.Chat, func.decrypt(msg.From.Id.ToString(), massive_message[1], redis));
                    break;
            }
        }
    }
}