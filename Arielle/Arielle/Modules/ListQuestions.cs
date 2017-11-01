using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle.Modules
{
    public class ListQuestions : ModuleBase<SocketCommandContext>
    {
        [Command("ShowQuestions")]
        public async Task ShowQuestions()
        {
            await Context.Channel.SendMessageAsync("List of all questions:");
            foreach (var q in Program.Questions)
            {
                await Context.Channel.SendMessageAsync($"{q.ID}: Question \"{q.Text}\" with Answer \"{q.Answer}\" of Category \"{q.Cat}\" and Subcategory \"{q.SubCat}\" of Difficulty \"{q.Diff}\"");
            }
        }
    }
}
