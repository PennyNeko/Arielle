using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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

                Question newQuestion = new Question((string)question.Text, string.Join(",",question.Answer),
                   string.Join(",", question.Cat), (string)question.Diff);
                questions.Add(newQuestion);
            }
        }

        public List<Question> GetQuestions()
        {
            return questions;
        }
    }
}
