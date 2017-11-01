using System.Threading.Tasks;
using Discord.Commands;
using System;

namespace Arielle.Modules
{
    public class RandomNumberGenerator : ModuleBase<SocketCommandContext>
    {
        [Command("Rng")]
        public async Task RandomGeneration(int start , int end)
        {
            Random random = new Random();
            int randomNumber = random.Next(start, end+1);
            await Context.Channel.SendMessageAsync(randomNumber.ToString());
        }
        [Command("Rng")]
        public async Task RandomGeneration(int end)
        {
            int start = 0;
            Random random = new Random();
            int randomNumber = random.Next(start, end + 1);
            await Context.Channel.SendMessageAsync(randomNumber.ToString());
        }
    }
}
