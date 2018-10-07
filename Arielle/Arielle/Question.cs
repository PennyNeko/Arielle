using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle
{
    class Question
    {
        public enum Category { Classes, Bosses, Outfits, Items, Lann, Fiona, Evie, Hurk, Karok, Kai, Vella, Arisha, Delia, Lynn, Miri, Sylas, Season3, Season2, Season1, Scrolls }
        public enum Difficulty { Easy, Medium, Hard }
        public Question(string text, string answer,string cat, string diff)
        {
            Text = text;
            List<Category> category = new List<Category>();
            string[] categories = cat.Split(',');
            foreach (string c in categories)
            {
                category.Add((Category)Enum.Parse(typeof(Category), c));
            }
            Cat = category;
            List<string> answerList = new List<string>();
            string[] answers = answer.Split(',');
            foreach (string s in answers)
            {
                answerList.Add(s.ToLower());
            }
            Answer = answerList;
            Diff = (Difficulty)Enum.Parse(typeof(Difficulty), diff);
        }
        public string Text
        {
            set;
            get;
        }
        public List<string> Answer
        {
            set;
            get;
        }
        public List<Category> Cat
        {
            set;
            get;
        }
        public Difficulty Diff
        {
            set;
            get;
        }
    }
}