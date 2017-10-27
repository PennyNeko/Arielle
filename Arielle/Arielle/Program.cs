using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Arielle.SaveLoad;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Arielle
{
    public class Program
    {
        List<User> users = new List<User>();
        List<Question> questions = new List<Question>();
        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public async Task StartAsync()
        {
            //convert Enums to Strings (instead of Integer) globally
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = false });
                return settings;
            });

            _client = new DiscordSocketClient();
            _handler = new CommandHandler(_client);
            
            string token = "MzcyODEzNjQwNjk4NjkxNTg0.DNJpYA.G1Gt0s12ekKpSwvSbqnWrgnOysA";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            //Load the game users and place in list "users"
            //LoadUsers loadedUsers = new LoadUsers();
            //users = loadedUsers.GetUsers();

            LoadQuestions loadedQuestions = new LoadQuestions();
            questions = loadedQuestions.GetQuestions();

            //Save the game users from list "users" to JSON file Users.json
            //SaveUsers savedUsers = new SaveUsers(users);

            SaveQuestions savedQuestions = new SaveQuestions(questions);
            await Task.Delay(-1);
        }
    }
}
