using Arielle.SaveLoad;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;

namespace Arielle.Modules
{
    public class QuestionCommands : ModuleBase<SocketCommandContext>
    {
        private static Timer askedQuestionTimer;
        private static Timer quizTimer;
        private EmbedBuilder quizAnswer = new EmbedBuilder().WithAuthor(a => a.Name = "Arielle");
        private static Random random = new Random();
        Question randomQuestion;
        private static bool isQuestionRunning = false;
        private static bool isQuizRunning = false;

        [Command("AddQuestion")]
        public async Task AddNewQuestion(string text, string answer, string category,  string difficulty)
        {
            //Check if question already exists
            foreach (Question q in Program.Questions)
            {
                if (q.Text == text)
                {
                    await Context.Channel.SendMessageAsync($"Question \"{text}\" already exists!");
                    return; 
                }
            }
            Program.Questions.Add(new Question(text, answer, category, difficulty));
            await Context.Channel.SendMessageAsync($"Question \"{text}\" with correct answer \"{answer}\" has been successfully added");

            //Save game questions to JSON file
            SaveQuestions savedQuestions = new SaveQuestions(Program.Questions);
            return;
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
            askedQuestionTimer = new Timer(30 * 1000)
            {
                AutoReset = false
            };
            askedQuestionTimer.Elapsed += async (source, e) => await StopCurrentQuestion();
            askedQuestionTimer.Start();
        }

        private async Task StopCurrentQuestion()
        {
            Context.Client.MessageReceived -= QuizAnswersRecieved;
            await Context.Channel.SendMessageAsync("Stopped receiving replies.");
            askedQuestionTimer.Stop();
            askedQuestionTimer.Dispose();
            isQuestionRunning = false;
        }

        [Command("StartQuiz")]
        public async Task StartRunningQuizCommand()
        {
            if (!isQuizRunning && !isQuestionRunning)
            {
                quizTimer = new Timer(60 * 1000);
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
                quizTimer.Dispose();
                await Context.Channel.SendMessageAsync("Quiz ended.");
                if (isQuestionRunning)
                {

                    askedQuestionTimer.Stop();
                    askedQuestionTimer.Dispose();
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

                    if (!randomQuestion.Answer.Contains(msg.Content.ToLower()))
                        return;

                    User correctUser = null;
                    quizAnswer.WithTitle($"Correct answer \"{msg.Content}\" by {msg.Author.Username}");
                    quizAnswer.WithColor(Color.Green);
                    //await Context.Channel.SendMessageAsync($"Correct answer \"{randomQuestion.Answer}\" by {msg.Author.Mention}");

                    foreach (User user in Program.Users)
                    {
                        if (user.ID == msg.Author.Id)
                        {
                            correctUser = user;
                            break;
                        }
                    }

                    if (correctUser == null)
                    {
                        UserCommands addUser = new UserCommands();
                        addUser.AddNewUser(msg.Author.Id);
                        correctUser = Program.Users.Last();
                    }

                    AddPoint(correctUser,1);
                    await StopCurrentQuestion();
                    await Context.Channel.SendMessageAsync("", false, quizAnswer);
                }
                catch
                {
                }
            });
            return Task.CompletedTask;
        }

        private void AddPoint(User user, int points)
        {
            quizAnswer.WithDescription($"Current points: {user.Points}+{points}");
            //await Context.Channel.SendMessageAsync($"Current points: {user.Points}+1");
            user.Points += points;
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
                EmbedBuilder quizQuestion = new EmbedBuilder();
                quizQuestion.WithTitle($"{Program.Questions.IndexOf(q)}: {q.Text}");
                quizQuestion.WithDescription($"{string.Join(", ", q.Answer.ToArray())}\n {string.Join(", ", q.Cat.ToArray())}, {q.Diff}");
                //await channel.SendMessageAsync($"{Program.Questions.IndexOf(q)}: Question \"{q.Text}\" with Answer \"{string.Join(", ", q.Answer.ToArray())}\" of Categories \"{string.Join(", ", q.Cat.ToArray())}\" and Difficulty \"{q.Diff}\"");
                await channel.SendMessageAsync("", false, quizQuestion);
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
