using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle
{
    class User
    {
        public User(long id, int points)
        {
            ID = id;
            Points = points;
        }

        public long ID { get; set; }
        public int Points { get; set; }
    }
}
