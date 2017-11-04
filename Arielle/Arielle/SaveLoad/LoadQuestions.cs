using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arielle.Question;

namespace Arielle.SaveLoad
{
    class LoadQuestions
    {
        List<Question> questions = new List<Question>();
        public LoadQuestions()
        {
            string questionsFile = File.ReadAllText(@"..\..\JSON\Questions.json");
            foreach (var question in JsonConvert.DeserializeObject<dynamic>(questionsFile))
            {
                Question newQuestion = new Question((string)question.Text, (string)question.Answer, 
                    (Category) Enum.Parse(typeof(Category), (string)question.Cat), (SubCategory)Enum.Parse(typeof(SubCategory),
                    (string)question.SubCat), (Difficulty)Enum.Parse(typeof(Difficulty), (string)question.Diff));
                questions.Add(newQuestion);
            }
        }

        public List<Question> GetQuestions()
        {
            return questions;
        }
    }
}
