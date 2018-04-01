using Arielle.SaveLoad;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle.Modules
{
    public class UserCommands : ModuleBase<SocketCommandContext>
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
                    return;
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

        [Command("AddUser")]
        public async Task AddNewUser(ulong userId)
        {
            ulong userID = userId;
            bool userExists = false;
            foreach (User u in Program.Users)
            {
                if (u.ID == userID)
                {
                    await Context.Channel.SendMessageAsync($"User with ID {userID} already exists!");
                    userExists = true;
                    return;
                }
            }
            if (!userExists)
            {
                Program.Users.Add(new User(userID, 0));
                await Context.Channel.SendMessageAsync($"User with ID {userID} successfully added!");

                //Save the game users from list "users" to JSON file Users.json
                SaveUsers savedUsers = new SaveUsers(Program.Users);
            }
        }

        [Command("DeleteUser")]
        public async Task DeleteExistingUser()
        {
            bool userExists = false;
            ulong userId = Context.User.Id;
            foreach (User u in Program.Users)
            {
                if (u.ID == userId)
                {
                    Program.Users.Remove(u);
                    await Context.Channel.SendMessageAsync($"User {Context.User.Username} successfully deleted");
                    userExists = true;

                    //Save the game users from list "users" to JSON file Users.json
                    SaveUsers savedUsers = new SaveUsers(Program.Users);
                    return;
                }
            }

            if (!userExists)
            {
                await Context.Channel.SendMessageAsync($"User {Context.User.Username} does not exist in user list. Try adding with .AddUser command.");
            }
        }

        [Command("DeleteUser")]
        public async Task DeleteExistingUser(ulong userID)
        {
            bool userExists = false;
            ulong userId = userID;

            foreach (User u in Program.Users)
            {
                if (u.ID == userId)
                {
                    Program.Users.Remove(u);
                    await Context.Channel.SendMessageAsync($"User with ID {userId} successfully deleted");
                    userExists = true;

                    //Save the game users from list "users" to JSON file Users.json
                    SaveUsers savedUsers = new SaveUsers(Program.Users);
                    return;
                }
            }

            if (!userExists)
            {
                await Context.Channel.SendMessageAsync($"User with ID {userId} does not exist in user list. Try adding with .AddUser command.");
            }
        }
    }
}
