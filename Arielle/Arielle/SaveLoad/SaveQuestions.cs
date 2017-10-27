using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle.SaveLoad
{
    class SaveQuestions
    {
        string questionsPath = @"..\..\JSON\Questions.json";

        public SaveQuestions(List<Question> questions)
        {
            string jsonQuestions = JsonConvert.SerializeObject(questions);
            File.WriteAllText(questionsPath, jsonQuestions);
        }
    }
}
