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
            Embed embed = new EmbedBuilder()
                .WithColor(new Color(40, 40, 120))
                .WithAuthor(a => a.Name = "Arielle")
                .WithTitle("Embed!")
                .WithDescription("This is an embed.");
            
        }
    }
}
