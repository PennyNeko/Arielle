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
        public Question(string text, string answer,List<Category> cat, Difficulty diff)
        {
            Text = text;
            Answer = answer;
            Cat = cat;
            Diff = diff;
        }
        public string Text
        {
            set;
            get;
        }
        public string Answer
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