using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using System.Collections.Generic;
using System;

namespace Arielle.Modules
{
    public class CheckPoints : ModuleBase<SocketCommandContext>
    {
        [Command("CheckPoints")]
        public async Task CheckUserPoints()
        {
            ulong userID;
            userID = Context.User.Id;
            foreach (User u in Program.Users)
            {
                if (u.ID == userID)
                {
                    await Context.Channel.SendMessageAsync($"User's {Context.User.Username} points are: " + u.Points);
                    break;
                }
            }
        }

        [Command("CheckPoints")]
        public async Task CheckUserPoints(string userID)
        {
            //To-Do: get username instead of ID and retrieve ID from it 

            bool wasFound = false;
            foreach (User u in Program.Users)
            {
                if (u.ID == ulong.Parse(userID))
                {
                    IUser user = await Context.Channel.GetUserAsync(u.ID);
                    wasFound = true;
                    await Context.Channel.SendMessageAsync($"User's {user.Username} points are: " + u.Points);
                    break;
                }
            }
            if (!wasFound)
            {
                await Context.Channel.SendMessageAsync($"User \"{userID}\" was not found");
            }
        }
    }
}
