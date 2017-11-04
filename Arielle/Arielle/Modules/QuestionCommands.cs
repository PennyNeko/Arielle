using Arielle.SaveLoad;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arielle.Question;

namespace Arielle.Modules
{
    public class QuestionCommands : ModuleBase<SocketCommandContext>
    {
        static Random random = new Random();
        Question randomQuestion;

        [Command("AddQuestion")]
        public async Task AddNewQuestion(string text, string answer, string cat, string subCat, string diff)
        {
            bool questionExists = false;
            Category category = (Category)Enum.Parse(typeof(Category), cat);
            SubCategory subCategory = (SubCategory)Enum.Parse(typeof(SubCategory), subCat);
            Difficulty difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), diff);

            //Check if question already exists
            foreach (Question q in Program.Questions)
            {
                if (q.Text == text)
                {
                    await Context.Channel.SendMessageAsync($"Question \"{text}\" already exists!");
                    questionExists = true;
                    break;
                }
            }
            //if not create new Question object
            if (!questionExists)
            {
                Program.Questions.Add(new Question(text, answer, category, subCategory, difficulty));
                await Context.Channel.SendMessageAsync($"Question \"{text}\" with correct answer \"{answer}\" has been successfully added");

                //Save game questions to JSON file
                SaveQuestions savedQuestions = new SaveQuestions(Program.Questions);
            }

        }


        [Command("AskQuestion")]
        public async Task AskQuestion()
        {
            int i = 20;

            randomQuestion = GetRandomQuestion();
            await Context.Channel.SendMessageAsync($"Question: {randomQuestion.Text}");
            HandleQuizAnswers();

            
            /*while (i > 0)
            {
                await Task.Delay(1000);
                i--;
            }*/

            //await Stop();
        }

        private async Task Stop()
        {
            Context.Client.MessageReceived -= QuizAnswersRecieved;
            await Context.Channel.SendMessageAsync("Quiz ended.");
        }

        private Question GetRandomQuestion()
        {
            int randomPosition = random.Next(0, Program.Questions.Count);
            return Program.Questions[randomPosition];

        }

        private void HandleQuizAnswers()
        {
            Context.Client.MessageReceived += QuizAnswersRecieved;
        }

        private Task QuizAnswersRecieved(SocketMessage imsg)
        {
            var _ = Task.Run(async () =>
            {
                try
                {

                    if (imsg.Author.IsBot)
                        return;
                    var msg = imsg as SocketUserMessage;

                    if (msg.Content.ToLower() != randomQuestion.Answer.ToLower())
                        return;
                    await Context.Channel.SendMessageAsync($"Correct answer \"{randomQuestion.Answer}\" by @{msg.Author.Username}");
                    bool userFound = false;
                    for (int i = 0; i < Program.Users.Count; i++)
                    {
                        if (Program.Users[i].ID == msg.Author.Id)
                        {
                            await Context.Channel.SendMessageAsync($"Current points: {Program.Users[i].Points}+1");
                            Program.Users[i].Points++;
                            userFound = true;
                            SaveUsers savedUsers = new SaveUsers(Program.Users);
                            await Stop();
                        }
                    }

                    //To-Do: create the user if doesn't exist.

                    if (!userFound)
                    {
                        await Context.Channel.SendMessageAsync($"User @{msg.Author.Username} not in the list of Players. Try adding yourself with \".AddUser\" first");
                    }
                }
                catch
                {
                }
            });
            return Task.CompletedTask;
        }

        [Command("ShowQuestions")]
        public async Task ShowQuestions()
        {
            IMessageChannel channel = Context.Channel;
            if (channel is ITextChannel)
            {
                channel = await ((IGuildUser)Context.User).GetOrCreateDMChannelAsync();
            }
            await channel.SendMessageAsync("List of all questions:");
            foreach (var q in Program.Questions)
            {
                await channel.SendMessageAsync($"{Program.Questions.IndexOf(q)}: Question \"{q.Text}\" with Answer \"{q.Answer}\" of Category \"{q.Cat}\" and Subcategory \"{q.SubCat}\" of Difficulty \"{q.Diff}\"");
            }
        }
        [Command("DeleteQuestion")]
        public async Task DeleteQuestion(string questionId)
        {
            int qId = int.Parse(questionId);
            Question questionToDelete = Program.Questions[qId];
            if (Context.User.Id == Program.OwnerID)
            {
                await Context.Channel.SendMessageAsync($"Question No.{qId} successfully deleted!");
                Program.Questions.RemoveAt(qId);

                //Save game questions to JSON file
                SaveQuestions savedQuestions = new SaveQuestions(Program.Questions);
            } else
                await Context.Channel.SendMessageAsync("Only the owner can delete questions!");
            //To-Do: Have confirmation dialogue
        }
    }
        
}
