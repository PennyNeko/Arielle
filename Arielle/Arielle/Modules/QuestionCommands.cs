using Arielle.SaveLoad;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using static Arielle.Question;
using System.Timers;
using System.Linq;

namespace Arielle.Modules
{
    public class QuestionCommands : ModuleBase<SocketCommandContext>
    {
        private static Timer askedQuestionTimer = new Timer(30 * 1000);
        private static Timer quizTimer = new Timer(60 * 1000);
        private static Random random = new Random();
        Question randomQuestion;
        private static bool isQuestionRunning = false;
        private static bool isQuizRunning = false;

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
        public async Task AskQuestionCommand()
        {
            if (!isQuestionRunning && !isQuizRunning)
            {
                await AskRandomQuestion();
                StartQuestionTimer();
            }
            else
            {
                await Context.Channel.SendMessageAsync("A question or quiz is already running.");
            }

        }

        private async Task AskRandomQuestion()
        {
            isQuestionRunning = true;
            randomQuestion = GetRandomQuestion();
            await Context.Channel.SendMessageAsync($"Question: {randomQuestion.Text}");
            HandleQuizAnswers();
        }

        private void StartQuestionTimer()
        {
            askedQuestionTimer.AutoReset = false;
            askedQuestionTimer.Elapsed += async (source, e) => await StopCurrentQuestion();
            askedQuestionTimer.Start();
        }

        private async Task StopCurrentQuestion()
        {
            Context.Client.MessageReceived -= QuizAnswersRecieved;
            await Context.Channel.SendMessageAsync("Stopped receiving replies.");
            askedQuestionTimer.Stop();
            isQuestionRunning = false;
        }

        [Command("StartQuiz")]
        public async Task StartRunningQuizCommand()
        {
            if (!isQuizRunning && !isQuestionRunning)
            {
                isQuizRunning = true;
                await AskRandomQuestion();
                StartQuestionTimer();
                quizTimer.Start();
                quizTimer.Elapsed += async (source, e) => { await AskRandomQuestion(); StartQuestionTimer(); };
            }
            else
            {
                await Context.Channel.SendMessageAsync("A quiz or question is already running.");
            }
        }

        [Command("StopQuiz")]
        public async Task StopRunningQuiz()
        {
            if (isQuizRunning)
            {
                quizTimer.Stop();
                await Context.Channel.SendMessageAsync("Quiz ended.");
                if (isQuestionRunning)
                {

                    askedQuestionTimer.Stop();
                    await StopCurrentQuestion();

                }
                isQuizRunning = false;
            }
            else
            {
                await Context.Channel.SendMessageAsync("No quiz running.");
            }
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

                    await Context.Channel.SendMessageAsync($"Correct answer \"{randomQuestion.Answer}\" by {msg.Author.Mention}");

                    foreach (User user in Program.Users)
                    {
                        if (user.ID == msg.Author.Id)
                        {
                            await AddPoint(user);
                            await StopCurrentQuestion();
                            return;
                        }
                    }

                    //Add new user to the list of users
                    UserCommands addUser = new UserCommands();
                    await Context.Channel.SendMessageAsync($"User @{msg.Author.Mention} not in the list of Players.");
                    await addUser.AddNewUser(msg.Author.Id);
                    await AddPoint(Program.Users.Last());

                    await StopCurrentQuestion();
                }
                catch
                {
                }
            });
            return Task.CompletedTask;
        }

        private async Task AddPoint(User user)
        {
            await Context.Channel.SendMessageAsync($"Current points: {user.Points}+1");
            user.Points++;
            SaveUsers savedUsers = new SaveUsers(Program.Users);
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
