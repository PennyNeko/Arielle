using Arielle.SaveLoad;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle.Modules
{
    public class AddUser : ModuleBase<SocketCommandContext>
    {
        [Command("AddUser")]
        public async Task AddNewUser()
        {
            ulong userID;
            userID = Context.User.Id;
            bool userExists = false;
            foreach (User u in Program.Users)
            {
                if (u.ID == userID)
                {
                    await Context.Channel.SendMessageAsync($"User {Context.User.Username} already exists!");
                    userExists = true;
                    break;
                }
            }
            if (!userExists)
            {
                Program.Users.Add(new User(userID, 0));
                await Context.Channel.SendMessageAsync($"User {Context.User.Username} successfully added!");

                //Save the game users from list "users" to JSON file Users.json
                SaveUsers savedUsers = new SaveUsers(Program.Users);
            }
        }
    }
}
