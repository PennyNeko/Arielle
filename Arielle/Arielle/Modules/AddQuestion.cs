using Arielle.SaveLoad;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arielle.Question;

namespace Arielle.Modules
{
    public class AddQuestion : ModuleBase<SocketCommandContext>
    {
        [Command("AddQuestion")]
        public async Task AddNewQuestion(string text, string answer, string cat, string subCat, string diff)
        {
            bool questionExists = false;
            Category category = (Category)Enum.Parse(typeof(Category), cat);
            SubCategory subCategory = (SubCategory)Enum.Parse(typeof(SubCategory), subCat);
            Difficulty difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), diff);
            foreach (Question q in Program.Questions)
            {
                if (q.Text == text)
                {
                    await Context.Channel.SendMessageAsync($"Question \"{text}\" already exists!");
                    questionExists = true;
                    break;
                }
            }
            if (!questionExists)
            {
                ulong lastID = Program.Questions.Last().ID;
                Program.Questions.Add(new Question(lastID + 1, text, answer, category, subCategory, difficulty));
                await Context.Channel.SendMessageAsync($"Question \"{text}\" with correct answer \"{answer}\" has been successfully added");

                //Save game questions to JSON file
                SaveQuestions savedQuestions = new SaveQuestions(Program.Questions);
            }

        }
    }
}
