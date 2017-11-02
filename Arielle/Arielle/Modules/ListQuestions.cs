using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Arielle.Modules
{
    public class ListQuestions : ModuleBase<SocketCommandContext>
    {
        [Command("ShowQuestions")]
        public async Task ShowQuestions()
        {
            var channel = Context.Channel;
            IMessageChannel ch = await ((IGuildUser)Context.User).GetOrCreateDMChannelAsync();
            await ch.SendMessageAsync("List of all questions:");
            foreach (var q in Program.Questions)
            {
                await ch.SendMessageAsync($"{q.ID}: Question \"{q.Text}\" with Answer \"{q.Answer}\" of Category \"{q.Cat}\" and Subcategory \"{q.SubCat}\" of Difficulty \"{q.Diff}\"");
            }
        }
    }
}
