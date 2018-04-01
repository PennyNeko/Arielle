using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("Test")]
        public async Task Testing()
        {
            await Context.Channel.SendMessageAsync("This is a cool message <:vindiyes:374126672942989313>");
        }
    }   
}
