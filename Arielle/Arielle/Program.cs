using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Arielle.SaveLoad;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;

namespace Arielle
{
    public class Program
    {
        List<User> users = new List<User>();
        List<Question> questions = new List<Question>();
        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        private CommandHandlingService _handler;

        public async Task MainAsync()
        {
            //convert Enums to Strings (instead of Integer) globally
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = false });
                return settings;
            });
            
            _client = new DiscordSocketClient();
            _handler = new CommandHandlingService(_client);

            //var services = ConfigureServices();
            //services.GetRequiredService<LogService>();

            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["token"]);
            await _client.StartAsync();

            //Load the game users and place in list "users"
            LoadUsers loadedUsers = new LoadUsers();
            users = loadedUsers.GetUsers();
            
            //Load game questions
            LoadQuestions loadedQuestions = new LoadQuestions();
            questions = loadedQuestions.GetQuestions();

            //Save the game users from list "users" to JSON file Users.json
            SaveUsers savedUsers = new SaveUsers(users);

            //Save game questions to JSON file
            SaveQuestions savedQuestions = new SaveQuestions(questions);

            
            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                // Logging
                //.AddLogging()
                //.AddSingleton<LogService>()
                // Add additional services here...
                .BuildServiceProvider();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("exit");
        }
    }
}
