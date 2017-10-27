using System.Threading.Tasks;
using Discord.Commands;

namespace Arielle.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public Task Info()
            => ReplyAsync(
                $"Hello, I am a bot called {Context.Client.CurrentUser.Username} written in Discord.Net 1.0 and created by PennyNeko#5431\n");
    }
}