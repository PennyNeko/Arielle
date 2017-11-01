using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;

namespace Arielle
{
    public class CommandHandlingService
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private char charPrefix = '.';

        public CommandHandlingService(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var context = new SocketCommandContext(_client, message);

            int argPos = 0;
            if (message.HasCharPrefix(charPrefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
