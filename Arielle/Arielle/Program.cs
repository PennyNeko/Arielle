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
        static List<User> users = new List<User>();
        static List<Question> questions = new List<Question>();

        public static ulong OwnerID;

        private DiscordSocketClient _client;
        private CommandHandlingService _handler;
        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();


        internal static List<User> Users { get => users; set => users = value; }
        internal static List<Question> Questions { get => questions; set => questions = value; }

        public async Task MainAsync()
        {
            //convert Enums to Strings (instead of Integer) globally
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = false });
                return settings;
            });
            
            OwnerID = ulong.Parse(ConfigurationManager.AppSettings["ownerID"]);
            _client = new DiscordSocketClient();
            _handler = new CommandHandlingService(_client);

            //var services = ConfigureServices();
            //services.GetRequiredService<LogService>();

            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["token"]);
            await _client.StartAsync();

            //Load the game users and place in list "users"
            LoadUsers loadedUsers = new LoadUsers();
            Users = loadedUsers.GetUsers();
            
            //Load game questions
            LoadQuestions loadedQuestions = new LoadQuestions();
            Questions = loadedQuestions.GetQuestions();
            
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
